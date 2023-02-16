using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    [SerializeField] private int damage = 20;

    private void Start()
    {

    }

    /// <summary>
    /// When the animation ends, destroy self
    /// </summary>
    public void OnTimeOut()
    {
        //check if player is in explosion
        Destroy(parent);
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        // will have it check for player collision and deal damage using the players "damage" function
        // GameManager.instance.player.Damage(_damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
