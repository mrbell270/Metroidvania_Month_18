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
        states[currentStateIdx].OnStateEnter();
    }

    private void Update()
    {
        states[currentStateIdx].OnStateStay();
    }

    private void ChangeState(int idx)
    {
        states[currentStateIdx].OnStateExit();
        currentStateIdx = idx;
        states[currentStateIdx].OnStateEnter();
    }
}
