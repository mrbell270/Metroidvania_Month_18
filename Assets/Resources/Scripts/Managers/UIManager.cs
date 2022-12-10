using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [Header("States")]
    int currentStateIdx = 0;
    [SerializeField]
    List<UIState> states = new();

    public int CurrentStateIdx { get => currentStateIdx; set => currentStateIdx = value; }

    public static UIManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (UIState uis in states)
        {
            uis.OnStateExit();
        }
    }

    private void Start()
    {
        states[CurrentStateIdx].OnStateEnter();
    }

    private void Update()
    {
        states[CurrentStateIdx].OnStateStay();
    }

    public void ChangeState(int idx)
    {
        if (idx < states.Count)
        {
            states[CurrentStateIdx].OnStateExit();
            CurrentStateIdx = idx;
            states[CurrentStateIdx].OnStateEnter();
        }
    }
}
