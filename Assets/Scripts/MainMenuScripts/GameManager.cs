using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] GameObject _thePlayerObject;
    [SerializeField] Player _thePlayer;
    private Vector2 _endRoomPos;
    private static GameManager _instance = null;
    private ArrayList allRooms;
    // When there is more enemy types each will get their own list
    private List<GameObject> allDroneEnemies;

    private DroneBoss theBoss; //When adding more bosses, we should make a boss interface

    private float healthItemValue; // temp var 

    private BalanceVariables theVars;


    void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        allRooms = null;
        allDroneEnemies = null;

        theBoss = null;

        healthItemValue = 1f;

    }
     public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null!");
            }
           return _instance;
        }
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


    public void OnPlayerDeath(){
        Debug.Log("GameManagerScript: Warning - Calling OnPlayerDeath when it is not implemented");
    }

     public GameObject GetPlayerObject()
     {
        return _thePlayerObject;
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