using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceSlider : MonoBehaviour
{
    public Text balanceLevel;
    public Text label;
    public float value = .5f;
    public string dictionaryKey;
    public Dictionary<string,float> dictionary;
    /*
    *   Param newValue = The value from the slider on the options page
    *   this function in the future will also change the volume of all music in the game
    */
    public void SetValuetext(float newValue)
    {
        float _oldValue = value;
        balanceLevel.text = newValue.ToString();
        value = newValue;
        if(dictionaryKey=="General")
        {
            GameObject.Find("BalanceMenu").GetComponent<BalanceMenu>().GeneralSliderChange(value-_oldValue);
        }
    }

    /*
    *   this will update all the sliders
    */
    public float GeneralFix(float changeAmount)
    {
        if(dictionaryKey!="General")
        {
            value+=changeAmount;
            
            if(value<=0.1f)
            {
                value=0.1f;
            }
            else if (value >= 2.0f)
            {
                value=2.0f;
            }
        }
        balanceLevel.text = value.ToString(); 
        return value;
        
    }

}
