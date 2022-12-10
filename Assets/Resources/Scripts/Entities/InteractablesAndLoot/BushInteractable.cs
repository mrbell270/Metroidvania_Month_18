using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushInteractable : Interactable
{
    [SerializeField]
    Sprite active;
    void Start()
    {
        TestWeapon = TestBushWeapon;

        col = GetComponent<Collider2D>();
    }

    public override void Activate()
    {
        col.enabled = false;
        GetComponent<SpriteRenderer>().sprite = active;
        Debug.Log("Bush Active!");
    }
    public bool TestBushWeapon(Weapon att)
    {
        return att.Type == AttackType.Slice || att.Type == AttackType.Fire;
    }

    public override void Decline()
    {
        // TODO: Animate
    }
}
