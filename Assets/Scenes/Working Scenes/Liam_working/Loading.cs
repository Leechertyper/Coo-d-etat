using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float delayBeforeLoading = 1f;
    public Text leftKeyText;
    public Text rightKeyText;
    public Text upKeyText;
    public Text downKeyText;
    public Text fireKeyText;
    public Text dashKeyText;

    private void Start()
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        LoadControls();

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

    public void LoadControls()
    {
        if (PlayerPrefs.HasKey("upKey"))
        {
            upKeyText.text = "Up: " + PlayerPrefs.GetString("upKey");
        }

        if (PlayerPrefs.HasKey("downKey"))
        {
            downKeyText.text = "Down: " + PlayerPrefs.GetString("downKey");
        }

        if (PlayerPrefs.HasKey("leftKey"))
        {
            leftKeyText.text = "Left: " + PlayerPrefs.GetString("leftKey");
        }

        if (PlayerPrefs.HasKey("rightKey"))
        {
            rightKeyText.text = "Right: " + PlayerPrefs.GetString("rightKey");
        }

        if (PlayerPrefs.HasKey("fireKey"))
        {
            fireKeyText.text = "Shoot: " + PlayerPrefs.GetString("fireKey");
        }

        if (PlayerPrefs.HasKey("dashKey"))
        {
            dashKeyText.text = "Dash: " + PlayerPrefs.GetString("dashKey");
        }
    }
}
