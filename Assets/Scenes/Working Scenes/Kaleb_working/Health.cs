using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{


    public int maxHealth = 10;
    private int _health;

    private void Start()
    {
        _health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            if (_health - damage > 0)
            {
                _health -= damage;
            }
            else 
            { 
                _health = 0; 
            }
        }
    }

    public int GetHealth()
    {
        return _health;
    }

    public void SetHealth(int newHealth)
    {
        if(newHealth > 0)
        {
            if(newHealth < maxHealth)
            {
                _health = newHealth;
            }
            else
            {
                _health = maxHealth;
            }
        }  
    }
}
