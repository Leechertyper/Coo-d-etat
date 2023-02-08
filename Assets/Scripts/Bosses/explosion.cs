using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    private float _damage;

    private void Start()
    {
        _damage = 50; // this will be taken from the globals once the server is up and running
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
}
