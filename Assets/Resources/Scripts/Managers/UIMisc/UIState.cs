using System.Collections;
using UnityEngine;

public abstract class UIState : MonoBehaviour
{
    [SerializeField]
    GameObject uiObject;

    public GameObject UIObject { get => uiObject;}

    public abstract void OnStateEnter();
    public abstract void OnStateStay();
    public abstract void OnStateExit();
}
