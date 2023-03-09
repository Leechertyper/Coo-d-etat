using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathItems : MonoBehaviour
{


    private int _spawnChance = 30; // Here just in case its needed for stat stuff 

    public GameObject batteryItem;
    private int _timeSinceLastBattery = 0; //Keeps track of the amount of failed battery rolls

    private int _batteryTime = 3; // the amount refernce for failed rolls


    public void SpawnItem(Transform thePlace)
    {
        if(_timeSinceLastBattery >= _batteryTime)
        {
            GameObject aBattery = Instantiate(batteryItem,thePlace);
            _timeSinceLastBattery = 0;
        }
        else if(Random.Range(0,100)>70) // 30% chance
        {
            GameObject aBattery = Instantiate(batteryItem,thePlace);
            _timeSinceLastBattery = 0;
        }
        else
        {
            _timeSinceLastBattery ++;
        }

    } 

}
