using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelSaveState
{
    [SerializeField]
    bool isCleared = false;
    public Dictionary<string, bool> mechanisms = new();

    public LevelSaveState()
    {

    }

    public bool IsCleared { get => isCleared; set => isCleared = value; }
}