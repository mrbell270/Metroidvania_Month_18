using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeActor();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackSensor.isSensoring)
        {
            battleController.AttackVector = attackSensor.direction;
        }
        else
        {
            battleController.AttackVector = Vector2.zero;
        }
        if (battleController.AttackVector.magnitude > 0f)
        {
            battleController.Attack();
        }
    }
}
