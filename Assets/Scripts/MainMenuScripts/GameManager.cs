using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] GameObject _thePlayerObject;
    [SerializeField] Player _thePlayer;
    private Vector2 _endRoomPos;
    private ArrayList allRooms;
    // When there is more enemy types each will get their own list
    private List<GameObject> allDroneEnemies;

    private DroneBoss theBoss; //When adding more bosses, we should make a boss interface

    private float healthItemValue; // temp var 

    private BalanceVariables theVars;


    public static GameManager Instance; // A static reference to the GameManager instance

    void Awake()
    {
        if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        allRooms = null;
        allDroneEnemies = null;

        theBoss = null;

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
    public void SetAllDroneEnemies(List<GameObject> allBads)
    {
        if(allDroneEnemies == null)
        {
            this.allDroneEnemies = allBads;
        }
        else
        {
            Debug.Log("GameManagerScript: Warning - trying to set the array of enemies when the array is full"); 
        }
    }

    //Used to clear all enemies in the floor
    public void ClearAllEnemies()
    {
        if(allDroneEnemies == null)
        {
            Debug.Log("GameManagerScript: Warning - trying to clear the array of enemies when the array is empty"); 
        }
        else 
        {
            this.allDroneEnemies.Clear();
        }
    }


    public void setCurrentBoss(DroneBoss curBoss)
    {
        if(theBoss = null)
        {
            this.theBoss = curBoss;
        }
        else
        {
            Debug.Log("GameManagerScript: Warning - trying to set the boss when there is already a boss for this floor");
        }
    }

    public void ClearCurrentBoss()
    {
        if(theBoss = null)
        {
            Debug.Log("GameManagerScript: Warning - trying to clear the boss when there is no boss for the current floor");
        }
        else 
        {
            theBoss = null;
        }
        Debug.Log("GameManagerScript: Warning - ClearCurrentBoss is not implemented yet");
    }

    //The stat changing functions are broad, will need refactoring when/if more then one emeny is in game
    public void ChangeEnemyStats(float newHealth, float newDamage, float newMoveSpeed, float newAttackSpeed)
    {


        Debug.Log("GameManagerScript: Warning - ChangeEnemyStats is not implemented yet");
        foreach (GameObject aDrone in allDroneEnemies)
        {
            Debug.Log("+1 drone");
            aDrone.GetComponent<DroneAI>().ChangeMoveSpeed(newMoveSpeed);
            aDrone.GetComponent<DroneAI>().ChangeAttackSpeed(newAttackSpeed);
        }
    }
    


    public void ChangeBossStats(float newHealth, float newDamage, float newSpeed, float newAttackSpeed)
    {

        theBoss.SetMaxHealth(newHealth);
        theBoss.SetDamage(newDamage);
        theBoss.SetMoveSpeed(newSpeed);
        theBoss.SetAttackSpeed(newSpeed);
        
    
        Debug.Log("GameManagerScript: Warning - ChangeBossStats is not implemented yet");
    }

    // To only be called by bosses in their Awaken 
    public void SetBossStats() 
    {
        this.ChangeBossStats(BalanceVariables.bossHealth,BalanceVariables.bossDamage,BalanceVariables.bossMoveSpeed,BalanceVariables.bossAttackSpeed);

    }

    public void OnPlayerDeath(){
        Debug.Log("GameManagerScript: Warning - Calling OnPlayerDeath when it is not implemented");
    }

     public GameObject GetPlayerObject()
     {
        return _thePlayerObject;
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
           _thePlayer.GetComponent<Player>().TakeDamage(theDamage);
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

    public void SetEndRoomPos(Vector2 endPos)
    {
        _endRoomPos = endPos;
    }

    public Vector2 GetEndRoomPos()
    {
        return _endRoomPos;
    }

    // The return list elements, 
    // [0] player, [1] drone
    public List<float> GetPowerValues()
    {
        List<float> theReturn = new List<float>();
        //theReturn.Add(BalanceVariables.player["power"]);
        theReturn.Add(1);  //Remove this when the balanceVariables values get changed
        theReturn.Add(BalanceVariables.droneEnemy["lazerDamage"]);
        return theReturn;
    }
}