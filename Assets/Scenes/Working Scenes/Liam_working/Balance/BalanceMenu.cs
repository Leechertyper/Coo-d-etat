using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BalanceMenu : MonoBehaviour
{
    public bool startBalance = false;
    public bool gameIsPaused = false;
    public GameObject balanceMenuUI;
    public GameObject buttonPrefab;
    public GameObject buttonContent;
    private GameObject selectedButton;
    public Text remainingPointsText;
    public GameObject confirmButton;
    private Dictionary<string,float>  selectedDict;    
    private string selectedDictKey;
    private string selectedValue; 

    public static BalanceMenu Instance;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startBalance && Time.timeScale == 1f)
        {
            if(!gameIsPaused)
            {
                Pause();
            }
        }
    }

    /*
    *   When called, this will reset the scene (balance menu wise) and go to next floor
    */
    public void ResumeGame()
    {
        balanceMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        startBalance = false;
        Dictionary<string,bool> _temp = new Dictionary<string,bool>(BalanceVariables.seenDictionaries);
        foreach (KeyValuePair<string, bool> kvp in _temp)
        {
            if(kvp.Key!="player" && kvp.Key!="other")
            {
                BalanceVariables.seenDictionaries[kvp.Key] = false;
            }
        }
        GameManager.Instance.EndBalanceMenu();
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
        balanceMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        PopulateButtons();
    }

    private void SetRemainingPoints()
    {
        remainingPointsText.text = "Remaining Points: " + PointBalanceTimer.Instance.counter.ToString();
    }

    /*
    *   This will remove all the options buttons
    */
    void RemoveButtons()
    {
        foreach (Transform child in buttonContent.transform) {
            Destroy(child.gameObject);
        }

    }

    /*
    *   This function will populate the buttonContent with buttons that contain sections that the user has seen since the last balance
    */
    private void PopulateButtons()
    {
        
        confirmButton.SetActive(false);
        SetRemainingPoints();
        PointBalanceTimer.Instance.counter-=1;
        if(PointBalanceTimer.Instance.counter <0)
        {
            PointBalanceTimer.Instance.counter = 0;
        }
        RemoveButtons();
        Dictionary<string,Dictionary<string,float>> _tempDict = new Dictionary<string,Dictionary<string,float>>();
        int _numSeenDictionaries = 0;
        for(int i=0; i<BalanceVariables.dictionaryListStrings.Count; i++)
        {
            if(BalanceVariables.seenDictionaries[BalanceVariables.dictionaryListStrings[i]])
            {
                _tempDict.Add(BalanceVariables.dictionaryListStrings[i], BalanceVariables.dictionaryList[i]);
                _numSeenDictionaries+=1;

            }
 
        }
        List<string> _list = new List<string>();
        for(int i=0; i<Mathf.Min(3,_numSeenDictionaries); i++)
        {
            string _randomDict = GetRandomKeyFromDoubleDict(_tempDict);
            if(_list.Contains(_randomDict))
            {
                i-=1;
            }
            else
            {
                _list.Add(_randomDict);
                GameObject newButton = Instantiate(buttonPrefab, buttonContent.transform);
                newButton.GetComponent<Button>().GetComponentInChildren<Text>().text = _randomDict;
                newButton.GetComponent<Button>().onClick.AddListener(() => LoadSpecificDictButtons(_tempDict[_randomDict])); 
            }
            
 
        }
        
    }


    private void LoadSpecificDictButtons(Dictionary<string,float> temp){
        confirmButton.SetActive(true);
        RemoveButtons();
        List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>();
        for(int i=0; i<3; i++)
        {
            string _randomKey = GetRandomKeyFromDict(temp);
            string _modifyValue="buffValue";
            if(UnityEngine.Random.Range(0,2)==1)
            {
                _modifyValue="buffValue";
            }
            else
            {
                _modifyValue="nerfValue";
            }
            if(tupleList.Contains(new Tuple<string, string>(_randomKey, _modifyValue)))
            {
                i-=1;
            }
            else
            {
                tupleList.Add(new Tuple<string, string>(_randomKey, _modifyValue));
                GameObject newButton = Instantiate(buttonPrefab, buttonContent.transform);
                newButton.GetComponent<Button>().GetComponentInChildren<Text>().text =_randomKey;
                newButton.GetComponent<Button>().onClick.AddListener(() => SetSelectedButton(newButton,temp,_randomKey,_modifyValue)); 
                newButton.GetComponent<Button>().GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f);
                
                if(_modifyValue=="buffValue")
                {
                    newButton.GetComponent<Image>().color = new Color(67f/255f, 1f, 155f/255f);
                }
                else
                {
                    newButton.GetComponent<Image>().color = new Color(1f, .32f, .32f);
                }
            }
            
 
        }
    }

    public void ConfirmSelection()
    {
        if(selectedButton==null)
        {
            return;
        }
        ChangeBalanceVariables(selectedDict,selectedDictKey,selectedValue);
        selectedButton = null;
        selectedDict = null;
        selectedDictKey = null;
        selectedValue = null;
        if(PointBalanceTimer.Instance.counter >0)
        {
            PopulateButtons();
        }
        else
        {
            ResumeGame();
        }
    }

    private void SetSelectedButton(GameObject button,Dictionary<string,float> Dictionary, string key, string modifyValue)
    {
        if(selectedButton!=null)
        {
            selectedButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(selectedButton.GetComponent<RectTransform>().anchoredPosition.x, selectedButton.GetComponent<RectTransform>().anchoredPosition.y - 10f);
        }
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x, button.GetComponent<RectTransform>().anchoredPosition.y + 10f);
        selectedButton = button;
        selectedDict = Dictionary;
        selectedDictKey = key;
        selectedValue = modifyValue;
    }

    private void ChangeBalanceVariables(Dictionary<string,float> temp, string key, string modifyValue){
        GameManager.Instance.BalanceValue(temp,key, BalanceVariables.other[modifyValue]);
    }

    private string GetRandomKeyFromDoubleDict(Dictionary<string,Dictionary<string,float>> dictionary)
    {
        // Get a collection of keys from the dictionary
        List<string> keyList = new List<string>(dictionary.Keys);

        // Generate a random index into the key list
        int randomIndex = UnityEngine.Random.Range(0, keyList.Count);

        // Return the key at the random index
        return keyList[randomIndex];
    }

    private string GetRandomKeyFromDict(Dictionary<string,float> dictionary)
    {
        // Get a collection of keys from the dictionary
        List<string> keyList = new List<string>(dictionary.Keys);

        // Generate a random index into the key list
        int randomIndex = UnityEngine.Random.Range(0, keyList.Count);

        // Return the key at the random index
        return keyList[randomIndex];
    }

    public void Clickybutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
    }


}