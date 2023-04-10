using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazer : MonoBehaviour
{

    public Rigidbody2D rb;
    private float _dronePower;
    private void Awake()
    {
        _dronePower = BalanceVariables.droneEnemy["lazerDamage"] * (Mathf.Sqrt(GameManager.Instance.getLevelNum()));
    }

    // When off screen Destroy Lazer
    // NOTE* this only deletes when off camera including the inspector camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Walls")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(_dronePower);

            Destroy(gameObject);
        }
    }
}
