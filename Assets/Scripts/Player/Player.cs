using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private float _speed = 5f;
    private float _maxHealth = 100f;
    private float _health = 100f;
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
    [SerializeField] private AK.Wwise.RTPC _rtpc;
    //[SerializeField] private AK.Wwise.State _playerState;

    public Animation death;
    public GameObject hitParticles;

    private void Start()
    {
        AkSoundEngine.SetState("PlayerLife", "Alive");
        //AkSoundEngine.SetState("Music_State", "Normal_Room");
        //AkSoundEngine.PostEvent("Play_Controller_Switch", this.gameObject);

        _rtpc.SetGlobalValue(_health);
        AkSoundEngine.PostEvent("Play_Heartbeat", this.gameObject);

    }

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

    

    public void TakeDamage(float damage)
    {
        if (_health > 0 && !_isInvuln)
        {

            //StartCoroutine(subtractOverTime(damage, _health, 1f));
            _health -= (int) damage;
            _rtpc.SetGlobalValue(_health);
            AkSoundEngine.PostEvent("Play_Pigeon_Hurt", this.gameObject);
            AkSoundEngine.PostEvent("Play_Pigeon_Coos", this.gameObject);
            if (_health < 0)
            {
                _health = 0;
            }
            GotHit();
            UpdateHealthUI();
            _isInvuln = true;
        }
        if (_health == 0){
            AkSoundEngine.PostEvent("Stop_Heartbeat", this.gameObject);
            //AkSoundEngine.PostEvent("Stop_Controller_Switch", this.gameObject);
            AkSoundEngine.SetState("PlayerLife", "Defeated");
            if(death){
                death.Play("Death");
            }
            Destroy(this, 2.0f);
            SceneManager.LoadScene("Alpha Main");
        }      
    }

    private void UpdateHealthUI()
    {
        _healthChanging = true;
    }

    private void GotHit()
    {
        Vector3 vRotation = Vector3.zero;
        vRotation.x = 90;
        Quaternion qRotation = Quaternion.identity;
        qRotation.eulerAngles = vRotation;
        Instantiate(hitParticles, this.transform.position, qRotation);
    }

    public float GetHealth()
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
        if(_health > BalanceVariables.player["_maxHealth"])
        {
            _health = BalanceVariables.player["_maxHealth"];
        }
        UpdateHealthUI();
    }

    public float GetMaxHealth()
    {
        return BalanceVariables.player["_maxHealth"];
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        BalanceVariables.player["_maxHealth"] = newMaxHealth;
        UpdateHealthUI();
    }

    public void MakeMaxHealth()
    {
        _health = BalanceVariables.player["_maxHealth"];
        UpdateHealthUI();
    }

    public float GetSpeed()
    {
        return BalanceVariables.player["_speed"];
    }

    public void SetSpeed(float newSpeed)
    {
        BalanceVariables.player["_speed"] = newSpeed;
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
        if (_power > BalanceVariables.player["_maxPower"])
        {
            _power = Mathf.RoundToInt(BalanceVariables.player["_maxPower"]);
        }
    }
    
    public IEnumerator subtractOverTime(float subtractor, float total, float duration) {
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            float currentA = Mathf.Lerp(subtractor, 0f, t);
            float currentB = Mathf.Lerp(total, total - subtractor, t);
            float result = currentB - currentA;
            _rtpc.SetGlobalValue(result);
            elapsedTime = Time.time - startTime;
            yield return null;
        }
        _rtpc.SetGlobalValue(total - subtractor);
    }
}
