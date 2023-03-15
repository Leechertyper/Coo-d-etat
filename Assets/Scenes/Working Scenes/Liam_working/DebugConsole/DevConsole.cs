using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DevConsole : MonoBehaviour
{
    public GameObject prefab;
    public Transform content;
    public GameObject consoleUI;
    public GameObject buttonPrefab;
    public GameObject dronePrefab;
    public GameObject dogPrefab;
    // public GameObject piratePrefab; // Not implemented
    public GameObject bossPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(consoleUI.activeSelf)
            {
                consoleUI.SetActive(false);
                DepopulateConsole();
            }
                
            else
            {
                consoleUI.SetActive(true);
                PopulateConsole();
            }
        }
    }

    private void DepopulateConsole()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateConsole()
    {
        GameObject button = Instantiate(buttonPrefab, content);
        button.GetComponentInChildren<Text>().text = "Spawn Drone";
        button.GetComponent<Button>().onClick.AddListener(SpawnDrone);

        button = Instantiate(buttonPrefab, content);
        button.GetComponentInChildren<Text>().text = "Spawn Dog";
        button.GetComponent<Button>().onClick.AddListener(SpawnDog);

        button = Instantiate(buttonPrefab, content);
        button.GetComponentInChildren<Text>().text = "Spawn Pirate";
        button.GetComponent<Button>().onClick.AddListener(SpawnPirate);

        button = Instantiate(buttonPrefab, content);
        button.GetComponentInChildren<Text>().text = "Spawn Boss";
        button.GetComponent<Button>().onClick.AddListener(SpawnBoss);

        for (int i = 0; i < BalanceVariables.dictionaryList.Count; i++)
        {
            foreach (KeyValuePair<string, float> kvp in BalanceVariables.dictionaryList[i])
            {
                string key = kvp.Key;
                float value = kvp.Value;
                GameObject canvas = Instantiate(prefab, content);
                Text parentLabel = canvas.transform.Find("ParentLabel").GetComponent<Text>();
                Text subLabel = canvas.transform.Find("SubLabel").GetComponent<Text>();
                InputField inputField = canvas.transform.Find("InputBox").GetComponent<InputField>();

                parentLabel.text = BalanceVariables.dictionaryListStrings[i];
                subLabel.text = key;
                inputField.text = value.ToString();

                // Attach listener to input field value changed event
                inputField.onValueChanged.AddListener((string value) =>
                {
                    OnInputValueChanged(parentLabel.text, subLabel.text, value);
                });
            }
        }
    }

    private void Start()
    {
       DontDestroyOnLoad(this);
    }

    public void SpawnDrone()
    {
        if( SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            GameObject drone = Instantiate(dronePrefab);
        }
        Debug.Log("In order to spawn a drone, you must be in the game scene.");
        
    }

    public void SpawnDog()
    {
        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            GameObject dog = Instantiate(dogPrefab);
        }
    }

    public void SpawnPirate()
    {
        Debug.Log("Pirate spawning not implemented");
        // GameObject pirate = Instantiate(Resources.Load("PirateEnemy"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        // pirate.GetComponent<PirateAI>().enabled = true;
    }

    public void SpawnBoss()
    {
        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            GameObject boss = Instantiate(bossPrefab);
        }
    }

    private void OnInputValueChanged(string parentLabel, string subLabel, string value)
    {
        int index = BalanceVariables.dictionaryListStrings.IndexOf(parentLabel);
        BalanceVariables.dictionaryList[index][subLabel] = float.Parse(value);
    }
}
