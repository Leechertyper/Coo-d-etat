using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{

    public Rigidbody2D rb;

    // When off screen Destroy Lazer
    // NOTE* this only deletes when off camera including the inspector camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);   
    }
}
