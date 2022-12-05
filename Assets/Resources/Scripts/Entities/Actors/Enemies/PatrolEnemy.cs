using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    public Sensor followSensor;

    [Header("Features")]
    [SerializeField]
    bool canFollow = false;
    bool isAtacking = false;
    bool wasFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeActor();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackUpdate();
        MoveUpdate();
    }

    void AttackUpdate()
    {
        if (attackSensor.isSensoring)
        {
            isAtacking = true;
            ((PatrolMovement)movementController).Stop();
            battleController.AttackVector = attackSensor.direction;
        }
        else
        {
            isAtacking = battleController.IsBusy();
            battleController.AttackVector = Vector2.zero;
        }
        if (battleController.AttackVector.magnitude > 0f)
        {
            battleController.Attack();
        }
    }

    void MoveUpdate()
    {
        if (!isAtacking)
        {
            if (canFollow && followSensor.isSensoring)
            {
                wasFollowing = true;
                ((PatrolMovement)movementController).Follow(followSensor.position);
            }
            else
            {
                if (wasFollowing)
                {
                    ((PatrolMovement)movementController).Stop();
                    wasFollowing = false;
                }
                ((PatrolMovement)movementController).Patrol();
            }
        }
    }
}
