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
    public GameObject categoryPrefab;
    public GameObject decreasePrefab;
    public GameObject increasePrefab;
    public GameObject buttonContent;
    public Text remainingPointsText;
    public GameObject confirmButton;
    public GameObject backButton;

    List<string> listOfBalanceDict = new List<string>();
    List<(string, List<SelectedItems>)> listOfBalanceDictKeys = new List<(string, List<SelectedItems>)>();
    public static List<SelectedItems> selectedValues = new List<SelectedItems>();

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

    //this function will take in a string and return a string with the first character capitalized and a space before each capital letter
    private string CapitalizeString(string _string)
    {
        string _temp = _string;
        _temp = _temp[0].ToString().ToUpper() + _temp.Substring(1);
        for(int i=0; i<_temp.Length; i++)
        {
            if(char.IsUpper(_temp[i]))
            {
                _temp = _temp.Insert(i, " ");
                i+=1;
            }
        }
        return _temp;
    }


    /*
    *   This function will populate the buttonContent with buttons that contain sections that the user has seen since the last balance
    */
    private void PopulateButtons()
    {
        backButton.SetActive(false);
        confirmButton.SetActive(false);
        SetRemainingPoints();
        PointBalanceTimer.Instance.counter-=1;
        if(PointBalanceTimer.Instance.counter <0)
        {
            PointBalanceTimer.Instance.counter = 0;
        }
        RemoveButtons();
        Dictionary<string,Dictionary<string,float>> _tempDict = new Dictionary<string,Dictionary<string,float>>();
        if(listOfBalanceDict.Count>0)
        {
            foreach (string item in listOfBalanceDict)
            {
                GameObject newButton = Instantiate(categoryPrefab, buttonContent.transform);
                newButton.transform.Find("Desc").GetComponent<Text>().text = item;
                newButton.GetComponent<Button>().onClick.AddListener(() => DisplaySpecificDictButtons(item)); 
            }
        }
        else
        {
            
            int _numSeenDictionaries = 0;
            for(int i=0; i<BalanceVariables.dictionaryListStrings.Count; i++)
                if(BalanceVariables.seenDictionaries[BalanceVariables.dictionaryListStrings[i]])
                {
                    _tempDict.Add(BalanceVariables.dictionaryListStrings[i], BalanceVariables.dictionaryList[i]);
                    _numSeenDictionaries+=1;

                }
    
            
            for(int i=0; i<Mathf.Min(3,_numSeenDictionaries); i++)
            {
                string _randomDict = GetRandomKeyFromDoubleDict(_tempDict);
                if(listOfBalanceDict.Contains(_randomDict))
                {
                    i-=1;
                }
                else
                {
                    listOfBalanceDict.Add(_randomDict);
                    LoadSpecificDictButtons(BalanceVariables.dictionaryList[BalanceVariables.dictionaryListStrings.IndexOf(_randomDict)]);
                    GameObject newButton = Instantiate(categoryPrefab, buttonContent.transform);
                    newButton.transform.Find("Desc").GetComponent<Text>().text = CapitalizeString(_randomDict);
                    newButton.GetComponent<Button>().onClick.AddListener(() => DisplaySpecificDictButtons(_randomDict)); 
                }
    
            }
        }
        
        
    }

    private void DisplaySpecificDictButtons(string dict){
        confirmButton.SetActive(true);
        backButton.SetActive(true);
        RemoveButtons();
        for(int i=0; i<listOfBalanceDictKeys.Count; i++)
        {
            if(listOfBalanceDictKeys[i].Item1==dict)
            {
                foreach (SelectedItems item in listOfBalanceDictKeys[i].Item2)
                {
                    if(item.selectedValue=="buffValue")
                    {
                        GameObject newButton = Instantiate(increasePrefab, buttonContent.transform);
                        newButton.transform.Find("Desc").GetComponent<Text>().text = CapitalizeString(item.selectedDictKey);
                        newButton.GetComponent<Button>().onClick.AddListener(() => SetSelectedButton(newButton,item.selectedDict,item.selectedDictKey,item.selectedValue)); 
                    }
                    else
                    {
                        GameObject newButton = Instantiate(decreasePrefab, buttonContent.transform);
                        newButton.transform.Find("Desc").GetComponent<Text>().text = CapitalizeString(item.selectedDictKey);
                        newButton.GetComponent<Button>().onClick.AddListener(() => SetSelectedButton(newButton,item.selectedDict,item.selectedDictKey,item.selectedValue)); 
                    }
                }
            }
        }
    }

    private void LoadSpecificDictButtons(Dictionary<string,float> temp){
        List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>();
        listOfBalanceDictKeys.Add((BalanceVariables.dictionaryListStrings[BalanceVariables.dictionaryList.IndexOf(temp)], new List<SelectedItems>()));
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
                listOfBalanceDictKeys[listOfBalanceDictKeys.Count-1].Item2.Add(new SelectedItems(null,temp,_randomKey,_modifyValue));
            }
 
        }
    }
    public void GoBackButton()
    {
        PopulateButtons();
    }
    public void ConfirmSelection()
    {
        if(selectedValues.Count==0)
        {
            return;
        }
        foreach (SelectedItems item in selectedValues)
        {
            ChangeBalanceVariables(item.selectedDict,item.selectedDictKey,item.selectedValue);
        }
        selectedValues.Clear();
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
        bool _inSelected = false;
        foreach (SelectedItems item in selectedValues)
        {
            if(item.selectedButton == button)
            {
                _inSelected = true;
                break;
            }
        }
        if(_inSelected)
        {
            selectedValues.Remove(selectedValues.Find(x => x.selectedButton == button));
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x, button.GetComponent<RectTransform>().anchoredPosition.y - 10f);
        }
        else
        {
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(button.GetComponent<RectTransform>().anchoredPosition.x, button.GetComponent<RectTransform>().anchoredPosition.y + 10f);
            selectedValues.Add(new SelectedItems(button,Dictionary,key,modifyValue));
            
        }
    }

    private void ChangeBalanceVariables(Dictionary<string,float> temp, string key, string modifyValue){
        GameManager.Instance.BalanceValue(temp,key, BalanceVariables.other[modifyValue]);
    }

    private string GetRandomKeyFromDoubleDict(Dictionary<string,Dictionary<string,float>> dictionary)
    {
        List<string> keyList = new List<string>(dictionary.Keys);
        int randomIndex = UnityEngine.Random.Range(0, keyList.Count);
        return keyList[randomIndex];
    }

    private string GetRandomKeyFromDict(Dictionary<string,float> dictionary)
    {
        List<string> keyList = new List<string>(dictionary.Keys);
        int randomIndex = UnityEngine.Random.Range(0, keyList.Count);
        return keyList[randomIndex];
    }

    public void Clickybutton()
    {
        AkSoundEngine.PostEvent("Play_Hover_Click_1", this.gameObject);
    }
}
[Serializable]
public class SelectedItems
{
    public Dictionary<string,float>  selectedDict;    
    public string selectedDictKey;
    public string selectedValue; 
    public GameObject selectedButton;

    public SelectedItems(GameObject selectedButton,Dictionary<string,float> selectedDict, string selectedDictKey, string selectedValue)
    {
        this.selectedDict = selectedDict;
        this.selectedDictKey = selectedDictKey;
        this.selectedValue = selectedValue;
        this.selectedButton = selectedButton;
    }
}