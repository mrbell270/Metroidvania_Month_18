using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryWeapon : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;
    [SerializeField]
    List<RangedWeapon> battery;


    // Use this for initialization
    void Start()
    {
        battery = new(GetComponentsInChildren<RangedWeapon>());
    }

    public void Attack()
    { 
        foreach (RangedWeapon shooter in battery)
        {
            shooter.Animate(null, direction);
        }
    }
}
