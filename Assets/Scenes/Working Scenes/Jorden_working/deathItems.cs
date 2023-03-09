using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathItems : MonoBehaviour
{


    private int _spawnChance = 30; // Here just in case its needed for stat stuff 

    public GameObject batteryItem;

    public GameObject healthItem;

    //Keeps track of the amount of failed rolls
    private int _timeSinceLastBattery = 0;

    private int _timeSinceLastHealth = 0; 

    // the amount reference for failed rolls 
    private int _batteryTime = 3; 

    private int _healthTime = 4;

    private int _itemSpawnChance = 15;

    private int _totalItems = 2;


    //With this funtion, if battery and health time are at max then the battery will always spawn first, 
    // I would like to make a check to see which one is lower then give that item, but only if time allows 
    public void SpawnItem(Transform thePlace)
    {
        if(_timeSinceLastBattery >= _batteryTime) // Spawn battery if timer is up
        {
            GameObject aBattery = Instantiate(batteryItem,thePlace);
            _timeSinceLastBattery = 0;
            _healthTime ++;
        }
        else if(_timeSinceLastHealth >= _healthTime) // Spawn health if timer is up
        {
            GameObject aHealth = Instantiate(healthItem,thePlace);
            _timeSinceLastHealth = 0;
            _batteryTime ++;
        }
        else
        {
            int temp = Random.Range(0,100);

            //Default is 15% chance per item
            if(temp >= (100-_itemSpawnChance)) 
            {
                GameObject aBattery = Instantiate(batteryItem,thePlace);
                _timeSinceLastBattery = 0;
            }
            else if(temp <= _itemSpawnChance)
            {
                GameObject aHealth = Instantiate(healthItem,thePlace);
                _timeSinceLastHealth = 0;
            }

            
            
        }


    } 


    public void SetSpawnChance(int newValue)
    {
        if((newValue*_totalItems) > 100)
        {
            _itemSpawnChance = 100 / _totalItems;
            Debug.LogWarning("Warning: Trying to change the item spawn chance to something greater then 100%");
        }
        else
        {
            _itemSpawnChance = newValue;
        }
    }

}
