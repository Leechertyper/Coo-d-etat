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
    public GameObject sliderPrefab;
    public GameObject sliderContent;
    public GameObject sliderScrollView;
    public GameObject buttonScrollView;
    public GameObject confirmButton;
    public Text remainingPoints;

    // Update is called once per frame
    void Update()
    {
        if(startBalance && Time.timeScale == 1f)
        {
            Pause();
            
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
            if(kvp.Key!="player")
            {
                BalanceVariables.seenDictionaries[kvp.Key] = false;
            }
        }
        GameManager.Instance.EndBalanceMenu(this.gameObject);

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
        remainingPoints.text = "Remaining Points: " + PointBalanceTimer.Instance.counter.ToString();
    }

    /*
    *   When called, this will balance the variables using what the user gives then go to the next level
    */
    public void ConfirmSelection()
    {
        foreach (Transform child in sliderContent.transform) 
        {
            if (child != null && child.GetComponent<BalanceSlider>() != null && child.GetComponent<BalanceSlider>().dictionaryKey != "General")
            {
                GameManager.Instance.BalanceValue(child.GetComponent<BalanceSlider>().dictionary,child.GetComponent<BalanceSlider>().dictionaryKey, child.GetComponent<BalanceSlider>().value);
            }
        }
        if(PointBalanceTimer.Instance.counter >0)
        {
            PopulateButtons();
        }
        else
        {
            ResumeGame();
        }
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
    *   This function removes all the slider options
    */
    void RemoveSliders()
    {
        foreach (Transform child in sliderContent.transform) 
        {
            Destroy(child.gameObject);
        }
    }

    /*
    *   if the user changes the general setting, it will change all the sliders
    */
    public void GeneralSliderChange(float valueChange)
    {
        foreach (Transform child in sliderContent.transform) 
        {
            child.GetComponent<Slider>().SetValueWithoutNotify(child.GetComponent<BalanceSlider>().GeneralFix(valueChange)) ;
        }
    }

    /*
    *   This function will populate the buttonContent with buttons that contain sections that the user has seen since the last balance
    */
    private void PopulateButtons()
    {
        buttonScrollView.SetActive(true);
        sliderScrollView.SetActive(false);
        confirmButton.SetActive(false);
        SetRemainingPoints();
        PointBalanceTimer.Instance.counter-=1;
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
    *   This function will populate the sliderContent with buttons that contain sections that the user has seen since the last balance
    */
    private void PopulateSliders(Dictionary<string,float> dictionary)
    {
        RemoveSliders();
        buttonScrollView.SetActive(false);
        sliderScrollView.SetActive(true);
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
