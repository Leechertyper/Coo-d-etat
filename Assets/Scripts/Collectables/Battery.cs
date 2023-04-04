using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Battery")]
public class Battery : ItemEffect
{
    //the BalanceVariables.collectables["batteryAmount"] that a stat changes 
    //after battery is picked up
    public override void Apply(GameObject target)
    {
        //IDK what power is, maybe player damage??
        //target.GetComponent<Player>().IncreasePower(((int)BalanceVariables.collectables["batteryAmount"])); 
        AkSoundEngine.PostEvent("Play_Potential_Pickup_SFX", target);
        target.GetComponent<PlayerAttack>().AddAmmo(BalanceVariables.collectables["batteryAmount"]*GameObject.Find("ShopManager").GetComponent<Shop>().GetBatteryMultiplier());

    }
}
