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
    public GameObject gameManagerScript;
    public GameObject buttonPrefab;
    public GameObject buttonContent;
    public GameObject sliderPrefab;
    public GameObject sliderContent;
    public GameObject sliderParents;
    public GameObject buttonParents;
    public GameObject confirmButton;

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
    *   This function will populate the sliderContent with buttons that contain sections that the user has seen since the last balance
    */
    private void PopulateButtons()
    {
        buttonParents.SetActive(true);
        sliderParents.SetActive(false);
        confirmButton.SetActive(false);
        RemoveButtons();
        for(int i=0; i<BalanceVariables.dictionaryListStrings.Count; i++)
        {
            if(BalanceVariables.seenDictionaries[BalanceVariables.dictionaryListStrings[i]])
            {
                int temp = i;
                GameObject newButton = Instantiate(buttonPrefab, buttonContent.transform);
                newButton.GetComponent<Button>().GetComponentInChildren<Text>().text = BalanceVariables.dictionaryListStrings[i];
                newButton.GetComponent<Button>().onClick.AddListener(() => PopulateSliders(BalanceVariables.dictionaryList[temp]));
            }
            
        }
    }

    /*
    *   When called, this will reset the scene (balance menu wise) and go to next floor
    */
    public void NextLevel()
    {
        balanceMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        startBalance = false;
        Dictionary<string,bool> _temp = new Dictionary<string,bool>(BalanceVariables.seenDictionaries);
        foreach (KeyValuePair<string, bool> kvp in _temp)
        {
            if(kvp.Key!="player")
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
    *   When called, this will pause the game.
    */
    public void ConfirmSelection()
    {
        foreach (Transform child in sliderContent.transform) 
        {
            if(child.GetComponent<BalanceSlider>().dictionaryKey!="General")
            {
                gameManagerScript.GetComponent<newGameManager>().BalanceSection(child.GetComponent<BalanceSlider>().dictionary,child.GetComponent<BalanceSlider>().dictionaryKey, child.GetComponent<BalanceSlider>().value);
            }
            
        }
        NextLevel();
    }

    void RemoveButtons()
    {
        foreach (Transform child in buttonContent.transform) {
            Destroy(child.gameObject);
        }

    }

    public void GeneralSliderChange(float valueChange)
    {
        foreach (Transform child in sliderContent.transform) 
        {
            child.GetComponent<Slider>().SetValueWithoutNotify(child.GetComponent<BalanceSlider>().GeneralFix(valueChange)) ;
        }
    }

    void RemoveSliders()
    {
        foreach (Transform child in sliderContent.transform) 
        {
            Destroy(child.gameObject);
        }
    }

    /*
    *   
    */
    void PopulateSliders(Dictionary<string,float> dictionary)
    {
        RemoveSliders();
        buttonParents.SetActive(false);
        sliderParents.SetActive(true);
        confirmButton.SetActive(true);

        GameObject newGenralSlider = Instantiate(sliderPrefab, sliderContent.transform);
        newGenralSlider.GetComponent<BalanceSlider>().label.text = "General Change";
        newGenralSlider.GetComponent<BalanceSlider>().dictionaryKey = "General";
        newGenralSlider.GetComponent<BalanceSlider>().dictionary = dictionary;
        newGenralSlider.GetComponent<BalanceSlider>().balanceLevel.text = "1.0";

        foreach (KeyValuePair<string, float> kvp in dictionary)
        {
            GameObject newSlider = Instantiate(sliderPrefab, sliderContent.transform);
            newSlider.GetComponent<BalanceSlider>().label.text = kvp.Key;
            newSlider.GetComponent<BalanceSlider>().dictionaryKey = kvp.Key;
            newSlider.GetComponent<BalanceSlider>().dictionary = dictionary;
            newSlider.GetComponent<BalanceSlider>().balanceLevel.text = "1.0";
        }
    }

}
