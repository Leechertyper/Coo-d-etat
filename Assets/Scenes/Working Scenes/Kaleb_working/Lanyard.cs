using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Lanyard")]
public class Lanyard : ItemEffect
{
    //the amount that a stat changes 
    //after garlic is picked up
    public GameObject lanyard;
    public override void Apply(GameObject target)
    {
        GameObject spawn = Instantiate(lanyard);
        target.GetComponent<HingeJoint2D>().enabled = true;
        target.GetComponent<HingeJoint2D>().connectedBody = spawn.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();

    }
}
