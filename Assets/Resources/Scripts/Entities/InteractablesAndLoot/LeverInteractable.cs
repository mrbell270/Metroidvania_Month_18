using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteractable : Interactable
{
    [SerializeField]
    List<Mechanism> connectedMechanisms = new();

    public List<Mechanism> ConnectedMechanisms { get => connectedMechanisms; set => connectedMechanisms = value; }

    void Start()
    {
        TestWeapon = TestLeverWeapon;

        col = GetComponent<Collider2D>();
    }

    public override void Activate()
    {
        transform.Rotate(0, 180, 0);

        foreach (Mechanism mech in ConnectedMechanisms)
        {
            mech.ChangeState(!mech.IsOn);
        }
    }
    public override void Decline()
    {
        // Always activate
    }

    public bool TestLeverWeapon(Weapon att) => true;

}
