using System.Collections;
using UnityEngine;

public abstract class UIState : MonoBehaviour
{
    [SerializeField]
    GameObject uiObject;

    public GameObject UIObject { get => uiObject;}

    public virtual void OnStateEnter()
    {
        Debug.Log("Enter" + gameObject.name);
        UIObject.SetActive(true);
    }
    public abstract void OnStateStay();
    public virtual void OnStateExit()
    {
        Debug.Log("Exir" + gameObject.name);
        UIObject.SetActive(false);
    }
}
