using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : Enemy
{
    [SerializeField]
    List<LaserAttack> attackQueue = new List<LaserAttack>();
    int curAttackIdx;
    bool isBusy;
    IEnumerator lastCorotine;

    void Start()
    {
        InitializeActor();
        curAttackIdx = 0;
        isBusy = attackQueue.Count == 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackQueue.Count > 0 && !battleController.IsLocked && !isBusy && !battleController.IsBusy())
        {
            isBusy = true;
            lastCorotine = PrepareAttack(attackQueue[curAttackIdx]);
            StartCoroutine(lastCorotine);
            curAttackIdx = (curAttackIdx + 1) % attackQueue.Count;
        }

    }

    public override void SetDead()
    {
        base.SetDead();
        attackQueue.Clear();
        StopCoroutine(lastCorotine);
    }

    IEnumerator PrepareAttack(LaserAttack attack)
    {
        battleController.AttackVector = attack.direction;
        yield return new WaitForSeconds(attack.offset);
        battleController.Attack();
        isBusy = false;
    }
}

[Serializable]
struct LaserAttack
{
    public Vector2 direction;
    public float offset;
}
