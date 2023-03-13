using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathItems : MonoBehaviour
{
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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            this.SpawnItem(new Vector3(0,0,-1));
        }
    }

    //With this funtion, if battery and health time are at max then the battery will always spawn first, 
    // I would like to make a check to see which one is lower then give that item, but only if time allows 
    public void SpawnItem(Vector3 thePlace)
    {
        Debug.Log("CHECKING IF SPAWN ITEM");
        if(_timeSinceLastBattery >= _batteryTime) // Spawn battery if timer is up
        {
            Instantiate(batteryItem, thePlace,Quaternion.identity);
            _timeSinceLastBattery = 0;
            _timeSinceLastHealth ++;
        }
        else if(_timeSinceLastHealth >= _healthTime) // Spawn health if timer is up
        {
            Instantiate(healthItem, thePlace,Quaternion.identity);
            _timeSinceLastHealth = 0;
            _timeSinceLastBattery ++;
        }
        else
        {
            int temp = Random.Range(0,100);

            //Default is 15% chance per item
            if(temp >= (100-_itemSpawnChance)) 
            {
                Instantiate(batteryItem, thePlace,Quaternion.identity);
                _timeSinceLastBattery = 0;
            }
            else if(temp <= _itemSpawnChance)
            {
                Instantiate(healthItem, thePlace,Quaternion.identity);
                _timeSinceLastHealth = 0;
            }
            else
            {
                _timeSinceLastBattery ++;
                _timeSinceLastHealth ++;
            }

            
            
        }


    } 

    //For use with the boss's packages
    public void JustSpawnBattery(Vector3 thePlace)
    {
        Instantiate(batteryItem, thePlace,Quaternion.identity);
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
