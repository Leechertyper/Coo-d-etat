using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newGameManager : MonoBehaviour
{
    private bool balanceTimerEnded;
    private float timerStartTime;

    // Start is called before the first frame update
    void Start()
    {
        BalanceTimerStart();
    }

    // Update is called once per frame
    void Update()
    {
        BalanceTimer();
    }

    public void BalanceTimerStart()
    {
        balanceTimerEnded=false;
        timerStartTime = Time.time;
    }

    private void BalanceTimer()
    {
        if ((Time.time - timerStartTime) >= BalanceVariables.other["balancePointTimerSeconds"])
        {
            //use balanceTimerEnded in both death and level complete, 
            //if true in those events: call StartBalanceMenu()
            balanceTimerEnded=true;
            StartBalanceMenu();
        }
    }
    public void StartBalanceMenu()
    {
        GameObject.Find("BalanceMenu").GetComponent<BalanceMenu>().startBalance=true;
    }

    /*
    *  This will change all of the values in a dictionary using the balance value provided
    *   The actual equation is still in question
    */
    public void BalanceSection(Dictionary<string,float> dictionary,string dictionaryKey, float balanceValue)
    {
        dictionary[dictionaryKey] *= balanceValue;

    }

}