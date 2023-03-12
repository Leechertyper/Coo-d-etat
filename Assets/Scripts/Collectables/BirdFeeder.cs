using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/BirdFeeder")]
public class BirdFeeder : ItemEffect
{

    public override void Apply(GameObject target)
    {
        target.GetComponent<Player>().MakeMaxHealth();
        Debug.Log("Player used bird feeder");
    }
}
