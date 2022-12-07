using System.Collections;
using UnityEngine;

public class HolderMechanism : Mechanism
{
    [SerializeField]
    GameObject heldObject;

    private void Update()
    {
        if(heldObject == null )
        {
            ChangeState(true);
        }
        else
        {
            heldObject.SetActive(!IsOn);
        }
    }
}
