using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // All explosions
    [SerializeField] private GameObject[] explosions;

    /// <summary>
    /// Runs when the last animation finishes
    /// 
    /// Spawns an explosion and destroys self
    /// </summary>
    public void OnAnimEnded()
    {
        GameObject explosion = Instantiate(explosions[(int)Mathf.Round(Random.Range(0, explosions.Length - 1))]);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
