using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public delegate bool TestWeaponDelegate(Weapon att);
    private List<string> activatorTags = new List<string> { "PlayerWeapon" };
    public TestWeaponDelegate TestWeapon;
    public Collider2D col;

    public abstract void Activate();
    public abstract void Decline();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activatorTags.Contains(collision.gameObject.tag))
        {
            Weapon activatorWeapon = collision.gameObject.GetComponent<Weapon>();
            if (activatorWeapon != null)
            {
                if (TestWeapon(activatorWeapon))
                {
                    Activate();
                }
                else
                {
                    Decline();
                }
            }
        }
    }
}
