using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    /// <summary>
    /// When the animation ends, destroy self
    /// </summary>
    public void OnTimeOut()
    {
        //check if player is in explosion
        Destroy(parent);
    }
}
