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
    public bool seenEnemy = false;
    public bool seenBoss = false;
    public Button buffButtonOne;
    public Button nerfButtonOne;
    public Button buffButtonTwo;
    public Button nerfButtonTwo;


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
    *   When called, this will NextLevel the game and go to next floor
    */
    public void NextLevel()
    {
        balanceMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        startBalance = false;
        gameManagerScript.GetComponent<newGameManager>().BalanceTimerStart();
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
        AssignButtonListeners();
        balanceMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    /*
    *   This function will randomize the 2 of the 4 button options
    *   (player will stay the same for right now until more values to change)
    */
    void AssignButtonListeners()
    {
        if(seenEnemy && !seenBoss)
        {
            AssignEnemyButtons();
        }
        else if(!seenEnemy && seenBoss)
        {
            AssignBossButtons();
        }
        else{
            if(Random.Range(1, 3)==2)
            {
                AssignBossButtons();
            }
            else
            {
                AssignEnemyButtons();
            }
        }
        AssignPlayerButtons();
        
    }

    /*
    *   Assigns the buff and nerf second buttons to the enemy
    */
    void AssignEnemyButtons()
    {
        buffButtonTwo.onClick.AddListener(BuffEnemy);
        buffButtonTwo.GetComponentInChildren<Text>().text = "Buff Enemy";
        nerfButtonTwo.onClick.AddListener(NerfEnemy);
        nerfButtonTwo.GetComponentInChildren<Text>().text = "Nerf Enemy";
    }

    /*
    *   Assigns the buff and nerf second buttons to the boss
    */
    void AssignBossButtons()
    {
        buffButtonTwo.onClick.AddListener(BuffBoss);
        buffButtonTwo.GetComponentInChildren<Text>().text = "Buff Boss";
        nerfButtonTwo.onClick.AddListener(NerfBoss);
        nerfButtonTwo.GetComponentInChildren<Text>().text = "Nerf Boss";
    }

    /*
    *   Assigns the buff and nerf first buttons to the player
    */
    void AssignPlayerButtons()
    {
        buffButtonOne.onClick.AddListener(BuffPlayer);
        buffButtonOne.GetComponentInChildren<Text>().text = "Buff Player";
        nerfButtonOne.onClick.AddListener(NerfPlayer);
        nerfButtonOne.GetComponentInChildren<Text>().text = "Nerf Player";
    }

    /*
    *   This will call ChangePlayerStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void BuffPlayer()
    {
        gameManagerScript.GetComponent<newGameManager>().BalanceFullSection(BalanceVariables.player, BalanceVariables.other["buffValue"]);
        NextLevel();

    }

    /*
    *   This will call ChangePlayerStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void NerfPlayer()
    {
        gameManagerScript.GetComponent<newGameManager>().BalanceFullSection(BalanceVariables.player, BalanceVariables.other["nerfValue"]);
        NextLevel();
    }

    /*
    *   This will call ChangeEnemyStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void BuffEnemy()
    {
        gameManagerScript.GetComponent<newGameManager>().BalanceFullSection(BalanceVariables.droneEnemy, BalanceVariables.other["buffValue"]);
        NextLevel();
    }

    /*
    *   This will call ChangeEnemyStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void NerfEnemy()
    {
        gameManagerScript.GetComponent<newGameManager>().BalanceFullSection(BalanceVariables.droneEnemy, BalanceVariables.other["nerfValue"]);
        NextLevel();
    }

    /*
    *   This will call ChangeBossStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void BuffBoss()
    {
        gameManagerScript.GetComponent<newGameManager>().BalanceFullSection(BalanceVariables.droneBoss, BalanceVariables.other["buffValue"]);
        NextLevel();
    }

    /*
    *   This will call ChangeBossStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void NerfBoss()
    {
        gameManagerScript.GetComponent<newGameManager>().BalanceFullSection(BalanceVariables.droneBoss, BalanceVariables.other["nerfValue"]);
        NextLevel();
    }
}
