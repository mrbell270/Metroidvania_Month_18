using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class GameStateManager : SerializedMonoBehaviour
{
    private static GameStateManager instance;

    [Header("Save states")]
    [SerializeField]
    Dictionary<int, LevelSaveState> levelSaveStates = new();

    [Header("Timers")]
    [ShowInInspector, ReadOnly]
    private bool isTiming;
    [ShowInInspector, ReadOnly]
    private bool isPausedTiming;
    [ShowInInspector, ReadOnly]
    private float inGameTimer;

    public int currentGameScene;

    public static GameStateManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        
        isTiming = false;
        isPausedTiming = true;
        inGameTimer = 0f;
    }

    public void LoadLevelReady(int sceneIdx)
    {
        currentGameScene = sceneIdx;
    }

    public void LoadLevel(int sceneIdx)
    {
        if (levelSaveStates.ContainsKey(sceneIdx))
        {
            LevelSaveState curLevelState;
            levelSaveStates.TryGetValue(sceneIdx, out curLevelState);

            if (curLevelState != null)
            {
                List<Mechanism> mechs = new List<Mechanism>(FindObjectsOfType<Mechanism>().Where(m => (m.SceneIdx == sceneIdx)));
                foreach (Mechanism m in mechs)
                {
                    if (curLevelState.mechanisms.ContainsKey(m.MechName)) 
                    {
                        m.ChangeState(curLevelState.mechanisms[m.MechName]); 
                    }
                }
            }
        }
        else
        {
            levelSaveStates.Add(sceneIdx, new LevelSaveState());
        }
    }

    public void UnloadLevel(int sceneIdx)
    {
        levelSaveStates.TryGetValue(sceneIdx, out LevelSaveState curLevelState);

        if (curLevelState != null)
        {
            Enemy[] aliveEnemies = FindObjectsOfType<Enemy>();
            int aliveEnemyCount = aliveEnemies.Where(e => (e.gameObject.scene.buildIndex == sceneIdx && !e.IsDead)).Count();
            curLevelState.IsCleared |= aliveEnemyCount == 0;

            List<Mechanism> mechs = new List<Mechanism>(FindObjectsOfType<Mechanism>().Where(m => (m.SceneIdx == sceneIdx)));
            foreach (Mechanism m in mechs)
            {
                if (curLevelState.mechanisms.ContainsKey(m.MechName))
                {
                    curLevelState.mechanisms[m.MechName] = m.IsOn;
                }
                else
                {
                    curLevelState.mechanisms.Add(m.MechName, m.IsOn);
                }
            }
        }
    }

    public bool CanDropChest(int sceneIdx)
    {
        bool result = true;
        levelSaveStates.TryGetValue(sceneIdx, out LevelSaveState curLevel);

        if (curLevel != null)
        {
            result &= !curLevel.IsCleared;
        }

        return result;
    }

    public bool ShouldDropChest(int sceneIdx)
    {
        Enemy[] aliveEnemies = FindObjectsOfType<Enemy>();
        int aliveEnemyCount = aliveEnemies.Where(e => (e.gameObject.scene.buildIndex == sceneIdx && !e.IsDead)).Count();
        return aliveEnemyCount == 0;
    }


    private void StartTimer()
    {
        if (!isPausedTiming)
        {
            inGameTimer = 0f;
        }
        isTiming = true;
    }
    private void PauseTimer()
    {
        isPausedTiming = true;
        isTiming = false;
    }

    private void StopTimer()
    {
        isTiming = false;
        if (isTiming) Debug.Log(inGameTimer);
    }
}

