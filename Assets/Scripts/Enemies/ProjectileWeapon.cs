using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using theNameSpace;
/*
 * Changable values for balance
 *  
 *  float fireForce - Increases the speed of the Lazers fired (Currently only by enemy)
 */
public class ProjectileWeapon : MonoBehaviour
{

    public GameObject projectile;
    public Transform anchorPoint;
    public float fireForce;

    private void Update()
    {

    }

    /// <summary>
    /// Instantiates Lazer in direction of Weapon and add force to it
    /// </summary>
    /// Note* Force applied to lazer is increaded by change in fireForce in the inspector
    public void Shoot(float angle)
    {        
        GameObject newProjectile = Instantiate(projectile, anchorPoint.position, Quaternion.Euler(0,0,angle));
        newProjectile.GetComponent<Rigidbody2D>().AddForce(newProjectile.transform.up * fireForce, ForceMode2D.Impulse);
    }
}
