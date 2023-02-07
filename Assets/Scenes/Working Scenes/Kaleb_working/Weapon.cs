using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Changable values for balance
 *  
 *  float fireForce - Increases the speed of the Lazers fired (Currently only by enemy)
 */
public class Weapon : MonoBehaviour
{

    public GameObject lazer;
    public Transform shootPoint;
    public float fireForce;


    /// <summary>
    /// Instantiates Lazer in direction of Weapon and add force to it
    /// </summary>
    /// Note* Force applied to lazer is increaded by change in fireForce in the inspector
    public void Shoot()
    {
        GameObject projectile = Instantiate(lazer, shootPoint.position, shootPoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(shootPoint.up * fireForce, ForceMode2D.Impulse);
    }
}
