using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/BatteryCharger")]
public class BatteryCharger : ItemEffect
{
    //public int amount;
    public override void Apply(GameObject target) 
    {
        
        target.GetComponent<PlayerAttack>().MakeMaxAmmo();
        Debug.Log("Player used battery charger");

    }
}
