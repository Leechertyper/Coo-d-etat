using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLazer : MonoBehaviour
{

    public Rigidbody2D rb;
    private float _playerPower;

    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        List<float> tempList = _gameManager.GetPowerValues();
        _playerPower = tempList[0];
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
        if (collision.gameObject.tag == "Enemy")
        {   // Remove type casting later
            collision.gameObject.GetComponent<Health>().TakeDamage((int)_playerPower);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<DroneBoss>().TakeDamage(_playerPower);
            Destroy(gameObject);
        }
    }
}