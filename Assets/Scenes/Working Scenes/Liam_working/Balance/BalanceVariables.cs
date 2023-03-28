using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceVariables : MonoBehaviour
{
    
    
    public static Dictionary<string,float> droneBoss = new Dictionary<string,float>()
    {
        //found in DroneBoss.cs
        {"maxHealth",1000f},
        {"moveSpeed", 1f},
        {"timeBetweenMoves",5f},
        //found in explosion.cs
        {"explosionDamage",20},
    };
    public static Dictionary<string,float> droneEnemy = new Dictionary<string,float>()
    {
        //found in Health.cs
        {"maxHealth",100},
        //found in DroneAI.cs
        {"moveSpeed",2f},
        {"range", 8f},
        {"pauseTime",3},
        {"slowShotCD",1},
        {"fastShotCD",0.5f},
        //could not find in but should be added in Lazer.cs(right now it just damages using 1) 
        // or ProjectileWeapon.cs (fireForce?)
        {"lazerDamage",10f},
        {"lazerSpeed", 20f},
    };

    public static Dictionary<string,float> dogEnemy = new Dictionary<string,float>()
    {
        //found in Health.cs should have its own
        {"maxHealth",100},
        //nothing implemented
        {"moveSpeed",2f},
        {"leapDistance",15f},
        {"attackDamage",10},
    };
    public static Dictionary<string,float> player = new Dictionary<string,float>()
    {
        //found in player.cs
        {"speed",5f},
        {"maxHealth",100},
        //in PlayerAttack.cs
        {"attackDamage",10f}, // this one is not implemented
        {"attackSpeed", 0.2f},
        {"maxAmmo", 100f},
        {"lazerSpeed", 25f},
        {"dashCooldown", 2f},
        {"dashDistance", 3f},
    };
    public static Dictionary<string,float> collectables = new Dictionary<string,float>()
    {
        {"dropChance",1f},
        //found in Garlic.cs
        {"garlicAmount", 25f},
        //found in Battery.cs
        {"batteryAmount", 25f},
        
    };
    public static Dictionary<string,float> other = new Dictionary<string,float>()
    {
        //found in PointBalanceTimer.cs
        {"balancePointTimerSeconds",120},
        {"stepSize", 0.01f},
    };

    //mainly for knowing if they have been seen for balancing, dont need to be pushed to database
    public static Dictionary<string,bool> seenDictionaries = new Dictionary<string,bool>()
    {
        {"player",true},  
        {"droneEnemy",false},  
        {"dogEnemy",false},
        {"droneBoss",false},  
        {"collectables",true},  
        {"other",true},  
    };

    //mainly for balancing, dont need to be pushed to database
    public static List<string> dictionaryListStrings = new List<string>()
    {
        "player", "droneEnemy", "droneBoss", "dogEnemy",  "collectables", "other",
    };

    //mainly for balancing, dont need to be pushed to database
    public static List<Dictionary<string,float>> dictionaryList = new List<Dictionary<string,float>>()
    {
        player, droneEnemy, droneBoss, dogEnemy, collectables, other
    };

}
