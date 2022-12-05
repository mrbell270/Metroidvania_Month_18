using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTrigger : MonoBehaviour
{
    [SerializeField]
    int idx;

    Dictionary<int, AsyncOperation> preloadedScenes = new();

    public int Idx { get => idx; set => idx = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadScene(Idx));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(UnloadScene(Idx));
        }
    }

    IEnumerator LoadScene(int idx)
    {
        if (!SceneManager.GetSceneByBuildIndex(idx).isLoaded)
        {
            AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(idx, LoadSceneMode.Additive);
            asyncLoading.allowSceneActivation = false;
            preloadedScenes.Add(idx, asyncLoading);
            GameStateManager gsm = GameStateManager.GetInstance();
            while (gsm.currentGameScene != idx && !asyncLoading.allowSceneActivation)
            {
                yield return null;
            }
            asyncLoading.allowSceneActivation = true;
        }
    }

    IEnumerator UnloadScene(int idx)
    {
        AsyncOperation asyncLoading;
        preloadedScenes.TryGetValue(idx, out asyncLoading);
        if (asyncLoading != null)
        {
            asyncLoading.allowSceneActivation = true;
        }
        preloadedScenes.Remove(idx);
        while (!SceneManager.GetSceneByBuildIndex(idx).isLoaded)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(idx);
    }
}
