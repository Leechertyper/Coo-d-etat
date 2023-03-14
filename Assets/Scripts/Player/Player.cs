using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    private int _maxHealth = 100;
    private int _health = 100;
    private float _range = 1000f;
    private float _invulnTime = 1.1f;
    private bool _isInvuln = false;
    private bool _healthChanging = false;
    private bool _healthTrailChanging = false;

    [SerializeField] private Text _healthText;
    [SerializeField] private int _power;
    [SerializeField] private int _maxPower;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthTrail;


    public Animation death;
    public GameObject hitParticles;
    
    void Update()
    {
        if (_healthChanging)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, _health / _maxHealth, 3f * Time.deltaTime);
            if (Mathf.Round(healthBar.fillAmount * _maxHealth) == Mathf.Round(_health))
            {
                _healthChanging = false;
                _healthTrailChanging = true;
            }
            else
            {
                _healthTrailChanging = false;
            }
        }

        if (_healthTrailChanging)
        {
            healthTrail.fillAmount = Mathf.Lerp(healthTrail.fillAmount, _health / _maxHealth, 5f * Time.deltaTime);
            if (Mathf.Round(healthTrail.fillAmount * _maxHealth) == Mathf.Round(_health))
            {
                _healthTrailChanging = false;
            }

        }
        if (_isInvuln)
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
        Debug.Log("update health");
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

    public void AddHealth(int plusHealth)
    {
        _health += plusHealth;
        if(_health > _maxHealth)
        {
            _health = _maxHealth;
        }
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

    public void MakeMaxHealth()
    {
        _health = _maxHealth;
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
