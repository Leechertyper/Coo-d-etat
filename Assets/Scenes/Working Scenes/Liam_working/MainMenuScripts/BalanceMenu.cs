using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalanceMenu : MonoBehaviour
{
    [SerializeField] bool floorClear = false;
    public bool gameIsPaused = false;
    public GameObject balanceMenuUI;
    public GameObject gameManagerScript;

    // Update is called once per frame
    void Update()
    {
        if(floorClear)
        {
            if(!gameIsPaused)
            {
                Pause();
            }
        }
    }

    /*
    *   When called, this will Resume the game and go to next floor
    */
    public void Resume()
    {
        balanceMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        floorClear = false;
    }

    /*
    *   When called, this will pause the game.
    */
    void Pause()
    {
        balanceMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    /*
    *   This will call ChangePlayerStats to modify the values
    */
    public void BuffPlayer()
    {
        gameManagerScript.GetComponent<GameManagerScript>().ChangePlayerStats(BalanceVariables.playerHealth*=1.1f,BalanceVariables.playerDamage*=1.1f,BalanceVariables.playerMoveSpeed*=1.1f,BalanceVariables.playerAttackSpeed*=1.1f);
        Resume();
    }

    /*
    *   This will call ChangePlayerStats to modify the values
    */
    public void NerfPlayer()
    {
        gameManagerScript.GetComponent<GameManagerScript>().ChangePlayerStats(BalanceVariables.playerHealth*=.9f,BalanceVariables.playerDamage*=.9f,BalanceVariables.playerMoveSpeed*=.9f,BalanceVariables.playerAttackSpeed*=.9f);
        Resume();
    }

    /*
    *   This will call ChangeEnemyStats to modify the values
    */
    public void BuffEnemy()
    {
        gameManagerScript.GetComponent<GameManagerScript>().ChangeEnemyStats(BalanceVariables.enemyHealth*=1.1f,BalanceVariables.enemyDamage*=1.1f,BalanceVariables.enemyMoveSpeed*=1.1f,BalanceVariables.enemyAttackSpeed*=1.1f);
        Resume();
    }

    /*
    *   This will call ChangeEnemyStats to modify the values
    */
    public void NerfEnemy()
    {
        gameManagerScript.GetComponent<GameManagerScript>().ChangeEnemyStats(BalanceVariables.enemyHealth*=.9f,BalanceVariables.enemyDamage*=.9f,BalanceVariables.enemyMoveSpeed*=.9f,BalanceVariables.enemyAttackSpeed*=.9f);
        Resume();
    }
}
