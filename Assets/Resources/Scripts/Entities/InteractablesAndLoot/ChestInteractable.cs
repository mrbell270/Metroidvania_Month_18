using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : Interactable
{
    [SerializeField]
    GameObject skin;
    [SerializeField]
    bool isVisible = false;
    [SerializeField]
    bool canDropChest;
    [SerializeField]
    List<ChestDrop> drops = new();
    [SerializeField]
    bool isUnlocked = false;
    GameStateManager gsm;
    int sceneIdx;

    Animator animator;

    void Start()
    {
        gsm = GameStateManager.GetInstance();
        sceneIdx = gameObject.scene.buildIndex;
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        TestWeapon = TestChestWeapon;

        canDropChest = gsm.CanDropChest(sceneIdx);
        SetVisible(gsm.ShouldDropChest(sceneIdx));
    }

    private void Update()
    {
        if (canDropChest && !isVisible)
        {
            SetVisible(gsm.ShouldDropChest(sceneIdx));
        }
    }

    void SetVisible(bool isOn)
    {
        isVisible = isOn;
        skin.SetActive(isOn);
        if(isOn) animator.SetTrigger("spawned");
        col.enabled = isOn;
    }

    public override void Activate()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            StartCoroutine(ActivateRoutine());
        }
    }

    IEnumerator ActivateRoutine()
    {
        animator.SetTrigger("opened");
        yield return new WaitForSeconds(1f);

        foreach (ChestDrop drop in drops)
        {
            for (int i = 0; i < drop.amount; i++)
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
    public bool TestChestWeapon(Weapon att) => (att.Type == AttackType.Blunt || att.Type == AttackType.Slice);

    public override void Decline()
    {
        animator.SetTrigger("declined");
    }
}

[System.Serializable]
struct ChestDrop
{
    public GameObject loot;
    public int amount;
}
