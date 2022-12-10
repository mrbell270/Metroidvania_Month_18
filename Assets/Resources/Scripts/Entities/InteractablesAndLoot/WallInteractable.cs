using System.Collections;
using UnityEngine;

public class WallInteractable : Interactable
{
    void Start()
    {
        TestWeapon = TestWallWeapon;

        col = GetComponent<Collider2D>();
    }

    public override void Activate()
    {
        col.enabled = false;
        GetComponent<SpriteRenderer>().color = Color.clear;
    }
    public bool TestWallWeapon(Weapon att)
    {
        return att.Type == AttackType.Fire;
    }

    public override void Decline()
    {
        // TODO: animate SlightShake
    }
}
