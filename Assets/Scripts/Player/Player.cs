using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField]private float _health = 100f;
    private float _range = 1000f;
    private float _invulnTime = 1.1f;
    private bool _isInvuln = false;
    private bool _healthChanging = false;
    private bool _healthTrailChanging = false;
    public bool _hasKey = false;

    [SerializeField] private Text _healthText;
    //[SerializeField] private float _power;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthTrail;
    [SerializeField] private AK.Wwise.RTPC _rtpc;
    [SerializeField] private Canvas healthCanvas;
    //[SerializeField] private AK.Wwise.State _playerState;

    public Animation death;
    public GameObject hitParticles;

    public static Player Instance;

    public bool iFrame; // true if invulnerable, false if not

    private void Start()
    {
        AkSoundEngine.SetState("PlayerLife", "Alive");
        //AkSoundEngine.SetState("Music_State", "Normal_Room");
        //AkSoundEngine.PostEvent("Play_Controller_Switch", this.gameObject);
        _health = (BalanceVariables.player["maxHealth"] * returnShop().GetHealthMultiplier());
        _rtpc.SetGlobalValue(_health);
        //_power = (BalanceVariables.player["maxPower"]*returnShop().GetBatteryMultiplier());
        AkSoundEngine.PostEvent("Play_Heartbeat", this.gameObject);
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }


    }

    void Update()
    {
        if (GameManager.Instance.inGame)
        {
            if (_healthChanging)
            {
                healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, _health / (BalanceVariables.player["maxHealth"] * returnShop().GetHealthMultiplier()), 3f * Time.deltaTime);
                if (Mathf.Round(healthBar.fillAmount * (BalanceVariables.player["maxHealth"] * returnShop().GetHealthMultiplier())) == Mathf.Round(_health))
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
                healthTrail.fillAmount = Mathf.Lerp(healthTrail.fillAmount, _health / (BalanceVariables.player["maxHealth"] * returnShop().GetHealthMultiplier()), 5f * Time.deltaTime);
                if (Mathf.Round(healthTrail.fillAmount * (BalanceVariables.player["maxHealth"] * returnShop().GetHealthMultiplier())) == Mathf.Round(_health))
                {
                    _healthTrailChanging = false;
                }

            }
            if (_isInvuln)
            {
                _invulnTime -= Time.deltaTime;
                if (_invulnTime <= 0f)
                {
                    _isInvuln = false;
                    _invulnTime = 1f;
                }
            }
        }
    }

    

    public void TakeDamage(float damage)
    {
        if (iFrame) return;
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
            healthCanvas.GetComponent<Animator>().SetFloat("health", _health);
            
        }
        if (_health == 0){
            AkSoundEngine.PostEvent("Stop_Heartbeat", this.gameObject);
            //AkSoundEngine.PostEvent("Stop_Controller_Switch", this.gameObject);
            AkSoundEngine.SetState("PlayerLife", "Defeated");
            if(death){
                death.Play("Death");
            }

            _rtpc.SetGlobalValue(100);
            GameManager.Instance.OnPlayerDeath();
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
        if (_health > 100)
        {
            _rtpc.SetGlobalValue(100);
        }
        else
        {
            _rtpc.SetGlobalValue(_health);
        }
        
        if(_health > (BalanceVariables.player["maxHealth"]* returnShop().GetHealthMultiplier()))
        {
            _health = (BalanceVariables.player["maxHealth"]* returnShop().GetHealthMultiplier());
        }
        UpdateHealthUI();
    }

    public float GetMaxHealth()
    {
        return (BalanceVariables.player["maxHealth"]* returnShop().GetHealthMultiplier());
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        BalanceVariables.player["maxHealth"] = newMaxHealth* returnShop().GetHealthMultiplier();
        UpdateHealthUI();
    }

    public void MakeMaxHealth()
    {
        _health = (BalanceVariables.player["maxHealth"]* returnShop().GetHealthMultiplier());
        UpdateHealthUI();
    }

    public float GetSpeed()
    {
        return BalanceVariables.player["speed"];
    }

    public void SetSpeed(float newSpeed)
    {
        BalanceVariables.player["speed"] = newSpeed;
    }
    /**
    public float GetPower()
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
        if (_power > (BalanceVariables.player["maxPower"]*GameObject.Find("ShopManager").GetComponent<Shop>().GetBatteryMultiplier()))
        {
            _power = Mathf.RoundToInt((BalanceVariables.player["maxPower"]*GameObject.Find("ShopManager").GetComponent<Shop>().GetBatteryMultiplier()));
        }
    }
**/
    public void GetKey()
    {
        _hasKey = true;
    }

    public void UseKey()
    {
        _hasKey = false;
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

    public Shop returnShop()
    {
        return GameObject.Find("ShopManager").GetComponent<Shop>();
    }
}
