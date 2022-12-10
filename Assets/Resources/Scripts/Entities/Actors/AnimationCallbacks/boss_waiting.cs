
using System.Collections;
using UnityEngine;

public class boss_waiting : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ShovelBossEnemy boss = animator.GetComponent<ShovelBossEnemy>();
        if (boss != null)
        {
            boss.StartCoroutine(WaitAndChoosePath(animator, boss));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Dig");
        animator.ResetTrigger("Shift");
    }
    IEnumerator WaitAndChoosePath(Animator animator, ShovelBossEnemy boss)
    {
        yield return new WaitForSeconds(boss.WaitingTime);
        if (animator.GetBool("canDig"))
        {
            float path = Random.Range(0f, 1f);
            if (path < 0.25f)
            {
                animator.SetTrigger("Shift");
            }
            else
            {
                animator.SetBool("canDig", false);
                animator.SetTrigger("Dig"); 
            }
        }
        else
        {
            animator.SetBool("canDig", true);
            animator.SetTrigger("Shift");
        }
    }
}
