using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float delayBeforeLoading = 1f;

    private void Start()
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Start loading the next scene in the background
        StartCoroutine(LoadNextSceneAsync());
    }

    private IEnumerator LoadNextSceneAsync()
    {
        // Wait for a short delay before loading the next scene
        yield return new WaitForSeconds(delayBeforeLoading);

        // Start loading the next scene in the background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex +1);

        // Wait until the next scene is finished loading
        while (!asyncLoad.isDone)
        {
            // Update the progress bar or loading text as needed
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0.9f is the progress at which the scene is considered loaded
            Debug.Log("Loading progress: " + (int)(progress * 100) + "%");

            yield return null;
        }

        // Hide the loading screen
        loadingScreen.SetActive(false);
    }
}
