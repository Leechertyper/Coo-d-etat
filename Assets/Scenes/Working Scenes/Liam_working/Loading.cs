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
    [SerializeField] public Text factsText;

    private void Start()
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        LoadControls();
        PigeonFacts();

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

    public void PigeonFacts() {

        List<string> facts = new List<string>();

        facts.Add("Pigeons can control time! They do this out of spite.");
        facts.Add("Drones legally cannot fly within 5.6km of an airport. Pigeons, however, are above the law, and can fly wherever they want.");
        facts.Add("There are approximately 400 million pigeons worldwide, meaning every pigeon is responible for monitoring 20 people.");
        facts.Add("Pigeons are not yet classified as a bioweapon.");
        facts.Add("Pigeons are regularly spotted under bridges, as many of them are employed as structural inspectors.");
        facts.Add("The average pigeon eats 112 spiders a year. Pigeons Georg, who eats 10,000 spiders a year, is an outlier and should not be counted.");
        facts.Add("Cuneiform was actually created by pigeons stepping on wet clay. It took the Sumerians 500 years to figure out how to read it.");
        facts.Add("Garlic is beneficial for pigeons, as it helps them ward off vampire bats.");

        var num = Random.Range(0, 7);
        factsText.text = facts[num];
    }
}
