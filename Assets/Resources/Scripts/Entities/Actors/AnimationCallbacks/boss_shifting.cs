using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_shifting : StateMachineBehaviour
{
    ShovelBossEnemy boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<ShovelBossEnemy>();
        if (boss != null)
        {
            boss.GetMoveCommandRandom();
        }
        boss.Attack();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!(animInfo.IsName("ShiftIn") && animInfo.normalizedTime < 1))
        {
            boss.Move();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Wait");
    }

    IEnumerator ShiftAndWait(Animator animator, ShovelBossEnemy boss)
    {
        yield return new WaitForSeconds(boss.WaitingTime);
        animator.SetTrigger("Wait");
    }
}
