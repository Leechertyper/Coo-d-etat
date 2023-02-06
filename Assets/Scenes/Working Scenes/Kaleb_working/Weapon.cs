using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject lazer;
    public Transform shootPoint;

    public float fireForce;
    public void Shoot()
    {
        GameObject projectile = Instantiate(lazer, shootPoint.position, shootPoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(shootPoint.up * fireForce, ForceMode2D.Impulse);
    }
}
