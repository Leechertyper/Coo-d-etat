using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BalanceMenuOption3 : MonoBehaviour
{
    public bool startBalance = false;
    public bool gameIsPaused = false;
    public GameObject balanceMenuUI;
    public GameObject gameManagerScript;
    public GameObject buttonPrefab;
    public GameObject buttonContent;

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
        gameManagerScript.GetComponent<newGameManager>().BalanceTimerStart();

        
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
                newButton.GetComponent<Button>().GetComponentInChildren<Text>().text =_randomKey + "     " + _modifyValue;
                newButton.GetComponent<Button>().onClick.AddListener(() => ChangeBalanceVariables(temp, _randomKey, _modifyValue)); 
            }
            
 
        }
    }

    private void ChangeBalanceVariables(Dictionary<string,float> temp, string key, string modifyValue){
        gameManagerScript.GetComponent<newGameManager>().BalanceValue(temp,key, BalanceVariables.other[modifyValue]);
        ResumeGame();
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


}
