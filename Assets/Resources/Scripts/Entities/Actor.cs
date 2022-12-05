using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [Header("Actor Entities")]
    [SerializeField]
    public MovementController movementController;
    [SerializeField]
    public BattleController battleController;
    [SerializeField]
    public AnimationController animationController;


    public void InitializeActor()
    {
        movementController.ParentActor = this;
        battleController.ParentActor = this;
        animationController.ParentActor = this;

        movementController.IsLocked = true;
        battleController.IsLocked = true;
    }

    public abstract void SetDead();
}
