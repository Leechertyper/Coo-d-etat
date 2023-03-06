using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    private int _maxHealth = 10;
    private int _health = 10;
    private float _range = 1000f;
    private float _invulnTime = 1.1f;
    private bool _isInvuln = false;
    [SerializeField] private Text _healthText;
    [SerializeField] private int _power;
    [SerializeField] private int _maxPower;

    public Animation death;
    public GameObject hitParticles;

    void Update()
    {
        if(_isInvuln)
        {
            _invulnTime -= Time.deltaTime;
            if(_invulnTime <= 0f)
            {
                _isInvuln = false;
                _invulnTime = 1f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_health > 0 && !_isInvuln)
        {
            _health -= damage;
            if (_health < 0)
            {
                _health = 0;
            }
            GotHit();
            UpdateHealthUI();
            _isInvuln = true;
        }
        if (_health == 0){
            if(death){
                death.Play("Death");
            }
            Destroy(this, 2.0f);
            SceneManager.LoadScene("Alpha Main");
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

    public void IncreasePower(int powerAmount)
    {        
        _power += powerAmount;
        if (_power > _maxPower)
        {
            _power = _maxPower;
        }
    }
}
