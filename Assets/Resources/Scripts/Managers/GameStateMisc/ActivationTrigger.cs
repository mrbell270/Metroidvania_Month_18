using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivationTrigger : MonoBehaviour
{
    [SerializeField]
    int idx;

    Camera mainCamera;

    public int Idx { get => idx; set => idx = value; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.GetInstance().LoadLevelReady(Idx);
            StartCoroutine(ActivateScene(Idx));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameStateManager.GetInstance().UnloadLevel(Idx);
        StartCoroutine(DeactivateScene(Idx));
    }

    IEnumerator ActivateScene(int idx)
    {
        Scene sc = SceneManager.GetSceneByBuildIndex(idx);
        while (!sc.isLoaded)
        {
            yield return null;
        }

        // Move Camera
        mainCamera.transform.position = transform.position + new Vector3(0, 0, -20);

        SceneManager.SetActiveScene(sc);
        GameStateManager.GetInstance().LoadLevel(Idx);
    }

    IEnumerator DeactivateScene(int idx)
    {
        yield return null;
    }
}
