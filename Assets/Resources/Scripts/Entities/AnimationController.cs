using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    Actor parentActor;
    Animator anim;
    public const string movementParameter = "Movement";
    public const string deathParameter = "Death";
    public const string hitParameter = "Hit";

    public void InitializeAnimationController()
    {
        Anim = GetComponent<Animator>();
    }

    public Actor ParentActor { get => parentActor; set => parentActor = value; }
    public Animator Anim { get => anim; set => anim = value; }

    public virtual void AnimateDeath()
    {
        Anim.SetTrigger(deathParameter);
    }
    public virtual void AnimateHit()
    {
        Anim.SetTrigger(hitParameter);
    }

}
