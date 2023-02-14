using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BalanceMenu : MonoBehaviour
{
    public bool floorClear = false;
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
        if(floorClear && Time.timeScale == 1f)
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
        floorClear = false;
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
        float _newHealth = BalanceVariables.playerHealth*=1.1f;
        float _newDamage = BalanceVariables.playerDamage*=1.1f;
        float _newMoveSpeed = BalanceVariables.playerMoveSpeed*=1.1f;
        float _newAttackSpeed = BalanceVariables.playerAttackSpeed*=1.1f;
        gameManagerScript.GetComponent<GameManagerScript>().ChangePlayerStats(_newHealth,_newDamage,_newMoveSpeed,_newAttackSpeed);
        NextLevel();
    }

    /*
    *   This will call ChangePlayerStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void NerfPlayer()
    {
        float _newHealth = BalanceVariables.playerHealth*=.9f;
        float _newDamage = BalanceVariables.playerDamage*=.9f;
        float _newMoveSpeed = BalanceVariables.playerMoveSpeed*=.9f;
        float _newAttackSpeed = BalanceVariables.playerAttackSpeed*=.9f;
        gameManagerScript.GetComponent<GameManagerScript>().ChangePlayerStats(_newHealth,_newDamage,_newMoveSpeed,_newAttackSpeed);
        NextLevel();
    }

    /*
    *   This will call ChangeEnemyStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void BuffEnemy()
    {
        float _newHealth = BalanceVariables.enemyHealth*=1.1f;
        float _newDamage = BalanceVariables.enemyDamage*=1.1f;
        float _newMoveSpeed = BalanceVariables.enemyMoveSpeed*=1.1f;
        float _newAttackSpeed = BalanceVariables.enemyAttackSpeed*=1.1f;
        gameManagerScript.GetComponent<GameManagerScript>().ChangeEnemyStats(_newHealth,_newDamage,_newMoveSpeed,_newAttackSpeed);
        NextLevel();
    }

    /*
    *   This will call ChangeEnemyStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void NerfEnemy()
    {
        float _newHealth = BalanceVariables.enemyHealth*=.9f;
        float _newDamage = BalanceVariables.enemyDamage*=.9f;
        float _newMoveSpeed = BalanceVariables.enemyMoveSpeed*=.9f;
        float _newAttackSpeed = BalanceVariables.enemyAttackSpeed*=.9f;
        gameManagerScript.GetComponent<GameManagerScript>().ChangeEnemyStats(_newHealth,_newDamage,_newMoveSpeed,_newAttackSpeed);
        NextLevel();
    }

    /*
    *   This will call ChangeBossStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void BuffBoss()
    {
        float _newHealth = BalanceVariables.bossHealth*=1.1f;
        float _newDamage = BalanceVariables.bossDamage*=1.1f;
        float _newMoveSpeed = BalanceVariables.bossMoveSpeed*=1.1f;
        float _newAttackSpeed = BalanceVariables.bossAttackSpeed*=1.1f;
        gameManagerScript.GetComponent<GameManagerScript>().ChangeBossStats(_newHealth,_newDamage,_newMoveSpeed,_newAttackSpeed);
        NextLevel();
    }

    /*
    *   This will call ChangeBossStats to modify the values
    *   if we dont like BalanceVariables, we can change it to the actual files
    */
    public void NerfBoss()
    {
        float _newHealth = BalanceVariables.bossHealth*=.9f;
        float _newDamage = BalanceVariables.bossDamage*=.9f;
        float _newMoveSpeed = BalanceVariables.bossMoveSpeed*=.9f;
        float _newAttackSpeed = BalanceVariables.bossAttackSpeed*=.9f;
        gameManagerScript.GetComponent<GameManagerScript>().ChangeBossStats(_newHealth,_newDamage,_newMoveSpeed,_newAttackSpeed);
        NextLevel();
    }
}
