using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceVariables : MonoBehaviour
{
    public static Dictionary<string,float> droneBoss = new Dictionary<string,float>()
    {
        //found in DroneBoss.cs
        {"maxHealth",1000f},
        {"moveSpeed", 0.01f},
        {"timeBetweenMoves",5f},
        //found in explosion.cs
        {"explostionDamage",50},
    };
    public static Dictionary<string,float> droneEnemy = new Dictionary<string,float>()
    {
        //found in Health.cs
        {"maxHealth",10},
        //found in DroneAI.cs
        {"moveSpeed",2f},
        {"range", 3f},
        {"pauseTime",3},
        {"slowShotCD",1},
        {"fastShotCD",0.5f},
        //could not find in but should be added in Lazer.cs(right now it just damages using 1) 
        // or ProjectileWeapon.cs (fireForce?)
        {"lazerDamage",1},
    };
    public static Dictionary<string,float> player = new Dictionary<string,float>()
    {
        //found in player.cs
        {"speed",5f},
        {"rotationSpeed", 15f},
        {"maxHealth",10},
        {"range", 1000f},
        //power is damage
        {"power", 0f},
        //may remove???
        {"maxPower", 0f},
        //Not currently implemented
        {"attackSpeed", 0f},
    };
    public static Dictionary<string,float> collectables = new Dictionary<string,float>()
    {
        //dont know if found, can be added
        {"pickupRange",1f},
        {"dropChance",1f},
        //found in Garlic.cs
        {"garlicAmount", 1f},
        //found in Battery.cs
        {"batteryAmount", 1f},
        //not currently implemented
        {"armourAmount", 1f},
        
    };
    public static Dictionary<string,float> other = new Dictionary<string,float>()
    {
        //will be added to gamemanager
        {"balancePointTimerSeconds",300},  
        {"roomSpawnChance",0f},  
        {"buffValue",1.1f},
        {"nerfValue",0.9f}
    };

    //mainly for knowing if they have been seen for balancing, dont need to be pushed to database
    public static Dictionary<string,bool> seenDictionaries = new Dictionary<string,bool>()
    {
        {"player",true},  
        {"droneEnemy",true},  
        {"droneBoss",true},  
        {"collectables",true},  
        {"other",true},  
    };

    //mainly for balancing, dont need to be pushed to database
    public static List<string> dictionaryListStrings = new List<string>()
    {
        "player", "droneEnemy", "droneBoss", "collectables", "other",
    };

    //mainly for balancing, dont need to be pushed to database
    public static List<Dictionary<string,float>> dictionaryList = new List<Dictionary<string,float>>()
    {
        player, droneEnemy, droneBoss, collectables, other
    };


    ///<NOTE> THIS CAN ALL BE REMOVED AFTER BRANCH BalanceMenuThreeOptions IS IMPLEMENTED INTO DEV.///
    ///<NOTE> DO NOT USE THESE VALUES BELOW///
    public static float enemyHealth = 10f;
    public static float enemyDamage = 1f;
    public static float enemyMoveSpeed = 2f;
    public static float enemyAttackSpeed = 1f;

    public static float bossHealth = 10f;
    public static float bossDamage = 1f;
    public static float bossMoveSpeed = 2f;
    public static float bossAttackSpeed = 1f;

    public static float playerHealth = 10f;
    public static float playerDamage = 1f;
    public static float playerMoveSpeed = 5f;
    public static float playerAttackSpeed = 1f;
}
