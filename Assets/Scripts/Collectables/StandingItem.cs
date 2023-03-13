using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This does the same thing as Item.cs but this doesn't destroy the gameobject after use 
public class StandingItem : MonoBehaviour
{
    public ItemEffect itemEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            itemEffect.Apply(collision.gameObject);
        }
    }
}
