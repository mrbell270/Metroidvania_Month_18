using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControledMovement : MovementController
{
    private void Awake()
    {
        InitializeMovement();
    }
}