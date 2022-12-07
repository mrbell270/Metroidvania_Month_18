using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemInteractable : Interactable
{
    [SerializeField]
    int cost;
    [SerializeField]
    TextMeshPro costText;
    [SerializeField]
    List<Mechanism> connectedMechanisms = new();

    void Start()
    {
        TestWeapon = TestLeverWeapon;

        col = GetComponent<Collider2D>();
        costText.text = cost.ToString();
    }

    public override void Activate()
    {
        Player.GetInstance().Coins -= cost;

        foreach (Mechanism mech in connectedMechanisms)
        {
            mech.ChangeState(!mech.IsOn);
        }
    }
    public override void Decline()
    {
        // TODO: Animate
    }

    public bool TestLeverWeapon(Weapon att)
    {
        return Player.GetInstance().Coins >= cost;
    }


}