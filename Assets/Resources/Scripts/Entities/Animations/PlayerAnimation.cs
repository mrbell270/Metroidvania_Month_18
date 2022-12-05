using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AnimationController
{
    private void Start()
    {
        InitializeAnimationController();
    }

    private void Update()
    {
        Anim.SetBool(movementParameter, ParentActor.movementController.MovementVector.sqrMagnitude > 0.1f);
    }
}
