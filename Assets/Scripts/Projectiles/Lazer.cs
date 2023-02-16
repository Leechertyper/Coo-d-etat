using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{

    public Rigidbody2D rb;
    private int _power = 1;

    // When off screen Destroy Lazer
    // NOTE* this only deletes when off camera including the inspector camera
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && this.gameObject.tag != "PlayerProjectile")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(_power);
            Destroy(gameObject);
        }
    }
    public void SetPower(int power){
        _power = power;
    }
}
