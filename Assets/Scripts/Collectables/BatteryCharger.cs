using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/BatteryCharger")]
public class BatteryCharger : ItemEffect
{
    //public int amount;
    public override void Apply(GameObject target) 
    {
        AkSoundEngine.PostEvent("Play_Heal", target);
        target.GetComponent<PlayerAttack>().MakeMaxAmmo();
        target.GetComponent<Player>().MakeMaxHealth();
        Debug.Log("Player used battery charger");

    }
}
