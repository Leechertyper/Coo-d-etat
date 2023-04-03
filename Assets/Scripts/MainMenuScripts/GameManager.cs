using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject balanceMenu;

    public GlobalGrid Grid;

    public static GameManager Instance; // A static reference to the GameManager instance
    public static DatabaseManager dbInstance;

    private bool _skipBalance = false;

    //this is for now until next level implimented
    private bool _died = false;

    void Awake()
    {
        if (Instance == null) // If there is no instance already
        {
            //DontDestroyOnLoad(gameObject); // bugs the game with this line
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

        //Database needs the GetHostFound for it to work, this needs to happen after the Awake phase of initialization.
        dbInstance = this.gameObject.GetComponent<DatabaseManager>();
        if(PlayerPrefs.GetInt("BalanceDataBase") == 1 && dbInstance.GetHostFound())
        {
            for(int i = 0;i<BalanceVariables.dictionaryList.Count;i++)
            {
                List<string> keys = new List<string>(BalanceVariables.dictionaryList[i].Keys);
                foreach(string key in keys)
                {
                    float maxVal = dbInstance.GetMaxValue(BalanceVariables.dictionaryListStrings[i]+char.ToUpper(key[0]) + key.Substring(1));
                    float minVal = dbInstance.GetMinValue(BalanceVariables.dictionaryListStrings[i]+char.ToUpper(key[0]) + key.Substring(1));
                    float difference = maxVal - minVal;
                    float scaledValue = Tikhonov(dbInstance.GetSteps(BalanceVariables.dictionaryListStrings[i]+char.ToUpper(key[0]) + key.Substring(1)),10f,32f )*difference;
                    Debug.Log("Putting in :" + scaledValue + " with min value: " + minVal + " for " + BalanceVariables.dictionaryListStrings[i]+char.ToUpper(key[0]) + key.Substring(1));
                    BalanceVariables.dictionaryList[i][key] =minVal + scaledValue;
                }
            }
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
        //for now
        AkSoundEngine.SetRTPCValue("Dead_Mute", 0);
        AkSoundEngine.SetState("PlayerLife", "Defeated");
        _died = true;
        if (PointBalanceTimer.Instance.counter > 0 && !_skipBalance)
        {
            StartBalanceMenu();
        }
        else
        {
            _skipBalance = false;
            //update load next floor here
            EndBalanceMenu();
        }
    }

    public void GoToNextFloor(){
        if (PointBalanceTimer.Instance.counter > 0 && !_skipBalance)
        {
            StartBalanceMenu();
        }
        else{
            _skipBalance = false;
            //update load next floor here
            SceneManager.LoadScene(1);
        }
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

    /*
    *   This function is called when the balance menu needs to pop up (call it in BalanceTimer())
    */
    public void StartBalanceMenu()
    {
        balanceMenu.SetActive(true);
        balanceMenu.GetComponent<BalanceMenu>().startBalance = true;
    }

    public void EndBalanceMenu()
    {
        _skipBalance = true;
        balanceMenu.SetActive(false);
        balanceMenu.GetComponent<BalanceMenu>().startBalance = false;
        if(_died)
        {
            _died = false;
            if (GameObject.Find("ScoreManager").GetComponent<Score>() != null && Score.GetInstance().IsLocalHighScore())
            {
                SceneManager.LoadScene("HighScores");
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        else
        {
            GoToNextFloor();
        }
    }

    /*
    *  This will change all of the values in a dictionary using the balance value provided
    *   The actual equation is still in question
    */
    public void BalanceValue(Dictionary<string,float> dictionary,string dictionaryKey, float balanceValue)
    {

        
        string dictName = BalanceVariables.dictionaryListStrings[BalanceVariables.dictionaryList.IndexOf(dictionary)];
        float currSteps = dbInstance.GetSteps(dictName+char.ToUpper(dictionaryKey[0]) + dictionaryKey.Substring(1));
        
        if(PlayerPrefs.GetInt("BalanceDataBase") == 1 && dbInstance.GetHostFound())
        {
            if (dictName != "General"){
                dbInstance.UpdateSteps(dictName + char.ToUpper(dictionaryKey[0])+dictionaryKey.Substring(1), currSteps + balanceValue);
            }
            
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
    //For use with smaller values of val:ie(steps) - a type of sigmoid function
    //returns a value from 0-1 with respect to how steep you want it to be and where half the steps to reach the max.
    public float Tikhonov(float val, float steepness, float half){
        float scalar = Mathf.Pow(val,steepness)/(Mathf.Pow(val,steepness) + Mathf.Pow(half,steepness));
        return scalar;
    }
}