using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField] Player _thePlayer;

    private ArrayList allRooms, allEnemies; //Can use list if wanted
    
    //private Boss theBoss;

    private float healthItemValue; // temp var 

    //private List<Enemy> allEnemies; //Uncomment when there are enemies


    // Start is called before the first frame update
    void Start()
    {
        allRooms = null;
        allEnemies = null;

        //theBoss = null;

        healthItemValue = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    //My idea is that the pcg script will call this function when it has all the rooms generated
    public void SetRooms(ArrayList generatedRooms)
    {
        if(allRooms == null)
        {
            this.allRooms = generatedRooms;
        }
        else
        {
            Debug.Log("GameManagerScript: Warning - trying to set the array of rooms when the array is already full"); // Maybe not the best warning message
        }
        
    }

    //Returns the current rooms in the floor
    public ArrayList GetRooms()
    {
        if(allRooms == null)
        {
            Debug.Log("GameManagerScript: Warning - trying to get the array of rooms when the array is empty"); 
        }
        return this.allRooms;
        
    }

    public void ClearRooms()
    {
        if(allRooms == null)
        {
            Debug.Log("GameManagerScript: Warning - trying to clear the array of rooms when the array is empty");
        }
        else
        {
            this.allRooms.Clear();
            //Maybe clear all the enemies and boss when the rooms are cleared? Maybe just a new function ClearFloor for all clears? 
        }
    }

    //Used to give this script all the enemies in the floor
    public void SetAllEnemies(ArrayList allBads)
    {
        if(allEnemies == null)
        {
            this.allEnemies = allBads;
        }
        else
        {
            Debug.Log("GameManagerScript: Warning - trying to set the array of enemies when the array is full"); 
        }
    }

    //Used to clear all enemies in the floor
    public void ClearAllEnemies()
    {
        if(allEnemies == null)
        {
            Debug.Log("GameManagerScript: Warning - trying to clear the array of enemies when the array is empty"); 
        }
        else 
        {
            this.allEnemies.Clear();
        }
    }


    // public void setCurrentBoss(Boss curBoss)
    // {
    //     if(theBoss = null)
    //     {
    //         this.theBoss = curBoss;
    //     }
    //     else
    //     {
    //         Debug.Log("GameManagerScript: Warning - trying to set the boss when there is already a boss for this floor");
    //     }
    // }

    public void ClearCurrentBoss()
    {
    //     if(theBoss = null)
    //     {
    //         Debug.Log("GameManagerScript: Warning - trying to clear the boss when there is no boss for the current floor");
    //     }
    //     else 
    //     {
    //         theBoss.Clear();
    //     }
        Debug.Log("GameManagerScript: Warning - ClearCurrentBoss is not implemented yet");
    }

    //The stat changing functions are broad, will need refactoring when/if more then one emeny is in game
    public void ChangeEnemyStats(float newHealth, float newDamage, float newSpeed, float newAttackSpeed)
    {
        // foreach (Enemy aFoe in allEnemies)
        // {
        //     aFoe.ChangeHealth(newHealth);
        //     aFoe.ChangeDamage(newDamage);
        //     aFoe.ChangeMoveSpeed(newSpeed);
        //     aFoe.ChangeAttackSpeed(newAttackSpeed);
        // }

        Debug.Log("GameManagerScript: Warning - ChangeEnemyStats is not implemented yet");
    }
    
    public void ChangeBossStats( float newHealth, float newDamage, float newSpeed, float newAttackSpeed)
    {

        //     theBoss.setAttackSpeed();
        //     theBoss.setDamage();
        //     theBoss.SetMaxHealth();
        //     theBoss.setNewSpeed();

        Debug.Log("GameManagerScript: Warning - ChangeBossStats is not implemented yet");
    }

     public void ChangePlayerMaxHealth(int newHealth)
     {
        if(newHealth <= 0)
        {
            Debug.Log("GameManagerScript: Warning - Trying to apply one a negative max health value to the player"); 
        }
        else
        {
            _thePlayer.SetMaxHealth(newHealth);
        }
        
     }

    public void ChangePlayerSpeed(float newSpeed)
    {
        if(newSpeed <= 0)
        {
            Debug.Log("GameManagerScript: Warning - Trying to apply a negative speed value to the player"); 
        }
        else
        {
            _thePlayer.SetSpeed(newSpeed);
        }
       
    }

    public void DamagePlayer(int theDamage)
    {
        if(theDamage <= 0)
        {
            Debug.Log("GameManagerScript: Warning - Trying to apply negative damage to the player");
        }
        else
        {
            _thePlayer.TakeDamage(theDamage);
        }
    }

    public void ChangePlayerStats(float newHealth, float newDamage, float newSpeed, float newAttackSpeed)
    {
        if(newHealth <= 0 || newDamage <= 0 || newSpeed <= 0 || newAttackSpeed <= 0)
        {
            Debug.Log("GameManagerScript: Warning - Trying to apply one or more negative values to the player"); 
        }
        else 
        {
            _thePlayer.SetMaxHealth((int)newHealth);
            //_thePlayer.SetDamage(newDamage);
            _thePlayer.SetSpeed(newSpeed);
            //_thePlayer.SetAttackSpeed(newAttackSpeed);
        }

    }

    public void ChangeHealthItemValue(float newHealth)
    {
        if(newHealth <= 0)
        {
            Debug.Log("GameManagerScript: Warning - Trying to apply a negative values to the health items value"); 
        }
        else
        {
            healthItemValue = newHealth;
        }
       
    }

    public float GetHealthItemValue()
    {
        return healthItemValue;
    }

}