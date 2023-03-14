using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Battery")]
public class Battery : ItemEffect
{
    //the amount that a stat changes 
    //after battery is picked up
    public float amount;
    public override void Apply(GameObject target)
    {
        //IDK what power is, maybe player damage??
        //target.GetComponent<Player>().IncreasePower(((int)amount)); 
        target.GetComponent<PlayerAttack>().AddAmmo(amount);

    }
}
