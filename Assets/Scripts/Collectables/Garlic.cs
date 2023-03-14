using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Garlic")]
public class Garlic : ItemEffect
{
    //the amount that a stat changes 
    //after garlic is picked up
    public int amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Player>().AddHealth(amount);

    }
}
