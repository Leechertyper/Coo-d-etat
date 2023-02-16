using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    private int _maxHealth = 10;
    private int _health = 10;
    private float _range = 1000f;
    [SerializeField] private int _power;
    [SerializeField] private Text _healthText;
    public GameObject laser;
    public GameObject hitParticles;


    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            if (_health < 0)
            {
                _health = 0;
                
            }
            GotHit();
            UpdateHealthUI();
        }        
    }

    private void UpdateHealthUI()
    {
        _healthText.text = "HP " + _health + "/" + _maxHealth;
    }

    private void GotHit()
    {
        Vector3 vRotation = Vector3.zero;
        vRotation.x = 90;
        Quaternion qRotation = Quaternion.identity;
        qRotation.eulerAngles = vRotation;
        Instantiate(hitParticles, this.transform.position, qRotation);
    }

    public int GetHealth()
    {
        return _health;
    }

    public void SetHealth(int newHealth)
    {
        _health = newHealth;
        UpdateHealthUI();
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        UpdateHealthUI();
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public int GetPower()
    {
        return _power;
    }
    public void SetPower(int newPower)
    {
        _power = newPower;
    }
    
    public void Fire()
    {
        //TODO: Projectile fire function
        Debug.DrawRay(this.transform.position, this.transform.rotation *Vector3.up*_range, Color.red, 5.0f);
        Debug.Log("Hello");
    }
    
}
