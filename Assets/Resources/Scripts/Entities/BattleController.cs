using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleController : MonoBehaviour
{
    Actor parentActor;
    bool isLocked;
    bool init = true;

    Vector2 attackVector;
    [SerializeField]
    int maxHealthPoints;
    [SerializeField]
    int curHealthPoints;
    [SerializeField]
    List<string> attackerTags;

    [Header("Weapon")]
    [SerializeField]
    public Transform availableWeapons;
    Weapon currentWeapon;
    int currentWeaponIdx;

    [Header("Damage Modifiers")]
    bool isInDot = false;
    float dotTimer = 0f;
    float dotThreshold = 1f;

    public Actor ParentActor { get => parentActor; set => parentActor = value; }
    public Vector2 AttackVector { get => attackVector; set => attackVector = value; }
    public int MaxHealthPoints { get => maxHealthPoints; set => maxHealthPoints = value; }
    public int CurHealthPoints { get => curHealthPoints; set => curHealthPoints = value; }
    public List<string> AttackerTags { get => attackerTags; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public bool Init { get => init; set => init = value; }

    public bool IsBusy() => currentWeapon.IsBusy;

    public void SetAttackerTags(string tag) { attackerTags.Add(tag); }

    private void Update()
    {
        if (isLocked && Init)
        {
            AnimatorStateInfo animInfo = ParentActor.animationController.Anim.GetCurrentAnimatorStateInfo(0);
            if (!(animInfo.IsName("spawn") && animInfo.normalizedTime < 1))
            {
                isLocked = false;
                Init = false;
            }
        }
    }


    public void InitializeBattleController()
    {
        curHealthPoints = maxHealthPoints;
        currentWeaponIdx = 0;
        Weapon[] childWeapons = availableWeapons.GetComponentsInChildren<Weapon>();
        if (childWeapons.Length > 0)
        {
            currentWeapon = childWeapons[currentWeaponIdx];
        }
    }

    public virtual void TakeDamage(Weapon attack, bool mainDamage = true)
    {
        if (!IsLocked)
        {
            parentActor.animationController.AnimateHit();
            if (attack.Type == AttackType.Lethal && parentActor is Player)
            {
                CurHealthPoints -= 1;
                bool isDead = TestOrDie();
                if (!isDead) Player.GetInstance().RestoreLevelSpawn();
            }
            else
            {
                CurHealthPoints -= mainDamage ? attack.Damage : attack.DamageOverTime;
                TestOrDie();
            }
        }
    }

    bool TestOrDie()
    {
        if (curHealthPoints <= 0)
        {
            ParentActor.SetDead();
            return true;
        }
        return false;
    }

    public void Heal(int heal = 1)
    {
        if (!IsLocked)
        {
            CurHealthPoints = Mathf.Min(CurHealthPoints + heal, MaxHealthPoints);
        }
    }

    public void HealAll()
    {
        curHealthPoints = maxHealthPoints;
    }
    public void Attack()
    {
        if (!IsLocked)
        {
            currentWeapon.Animate(ParentActor, AttackVector);
        }
    }

    public void SetWeapon(int idx = -1)
    {
        Weapon[] childWeapons = availableWeapons.GetComponentsInChildren<Weapon>();
        if (idx == -1)
        {
            idx = childWeapons.Length - 1;
        }
        currentWeaponIdx = idx;
        currentWeapon = childWeapons[currentWeaponIdx];
    }

    public void SwitchWeapon()
    {
        Weapon[] childWeapons = availableWeapons.GetComponentsInChildren<Weapon>();
        currentWeaponIdx = (currentWeaponIdx + 1) % childWeapons.Length;
        currentWeapon = childWeapons[currentWeaponIdx];
    }

    public virtual void Deactivate()
    {
        isLocked = true;
        currentWeapon.GetComponent<Collider2D>().enabled = false;
        currentWeapon.StopAllCoroutines();
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        GetComponent<Collider2D>().enabled = false;
    }

    public virtual void DeactivateSoft()
    {
        isLocked = true;
        //GetComponent<Collider2D>().enabled = false;
    }

    public virtual void ReactivateSoft()
    {
        isLocked = false;
        //GetComponent<Collider2D>().enabled = true;
    }

    public void ResetAfterDeath()
    {
        attackVector = Vector2.zero;
        init = true;
        isLocked = true;
        HealAll();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackerTags.Contains(collision.gameObject.tag))
        {
            Weapon targetWeapon = collision.gameObject.GetComponent<Weapon>();
            if (targetWeapon != null)
            {
                TakeDamage(targetWeapon);
                if (targetWeapon.DamageOverTime > 0)
                {
                    isInDot = true;
                    dotTimer = 0f;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInDot)
        {
            if (attackerTags.Contains(collision.gameObject.tag))
            {
                Weapon targetWeapon = collision.gameObject.GetComponent<Weapon>();
                if (targetWeapon != null)
                {
                    dotTimer += Time.deltaTime;
                    if (dotTimer >= dotThreshold)
                    {
                        dotTimer = 0f;
                        TakeDamage(targetWeapon, mainDamage: false);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (attackerTags.Contains(collision.gameObject.tag))
        {
            Weapon targetWeapon = collision.gameObject.GetComponent<Weapon>();
            if (targetWeapon != null)
            {
                if (targetWeapon.DamageOverTime > 0)
                {
                    isInDot = false;
                }
            }
        }
    }
}
