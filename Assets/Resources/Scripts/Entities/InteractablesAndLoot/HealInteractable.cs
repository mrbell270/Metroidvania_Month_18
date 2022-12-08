using System.Collections;
using UnityEngine;

public class HealInteractable : Interactable
{
    Animator animator;
    [SerializeField]
    GameObject healPrefab;

    private void Start()
    {
        animator = GetComponent<Animator>();

        TestWeapon = TestHealWeapon;
    }
    public override void Activate()
    {
        if (animator != null) animator.SetTrigger("spawn");

        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(-0.5f, 0.5f);
        Vector3 direction = new Vector3(x, y, 0).normalized;
        GameObject hp = Instantiate(healPrefab, transform);
        hp.GetComponent<Loot>().IsBusy = true;
        StartCoroutine(LootMove(hp.transform, direction));
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

    public override void Decline()
    {
        // always activate
    }
    public bool TestHealWeapon(Weapon att) => true;
}