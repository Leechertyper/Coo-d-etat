using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Garlic")]
public class Garlic : ItemEffect
{
    //the BalanceVariables.collectables["garlicAmount"] that a stat changes 
    //after garlic is picked up
    public override void Apply(GameObject target)
    {
        target.GetComponent<Player>().AddHealth(Mathf.RoundToInt(BalanceVariables.collectables["garlicAmount"]));

    }
}
