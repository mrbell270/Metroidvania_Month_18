
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Actor
{
    [SerializeField]
    public Sensor attackSensor;
    bool isDead = false;

    [Header("Drops")]
    [SerializeField]
    List<Drop> drops = new List<Drop>();

    public bool IsDead { get => isDead; set => isDead = value; }

    public override void SetDead()
    {
        isDead = true;

        animationController.AnimateDeath();

        movementController.Deactivate();
        battleController.Deactivate();

        StopAllCoroutines();

        Drop();
    }

    void Drop()
    {
        foreach(Drop drop in drops)
        {
            float dice = Random.Range(0f, 1f);
            if (dice < drop.chance)
            {
                float x = Random.Range(-0.5f, 0.5f);
                float y = Random.Range(-0.5f, 0.5f);
                Vector3 direction = new Vector3(x, y, 0).normalized;
                GameObject d = Instantiate(drop.loot, transform);
                StartCoroutine(LootMove(d.transform, direction));
            }
        }
    }

    IEnumerator LootMove(Transform loot, Vector3 direction)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            loot.localPosition = Vector3.Lerp(loot.localPosition, direction, timer);
            yield return null;
        }
        YSortStatic ysort = loot.gameObject.GetComponent<YSortStatic>();
        if (ysort != null) ysort.Recalibrate();
        Loot lootloot = loot.gameObject.GetComponent<Loot>();
        if (lootloot != null) lootloot.IsBusy = false;
    }
}

[System.Serializable]
struct Drop
{
    public GameObject loot;
    [Range(0, 1)] public float chance;
}
