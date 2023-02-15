using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    private float _rotationSpeed = 15f;
    private int _maxHealth = 10;
    private int _health = 10;
    private float _range = 1000f;
    private Rigidbody2D _rigidBody;
    private Vector2 _movement;
    [SerializeField] private int _power;
    [SerializeField] private int _maxPower;
    [SerializeField] private Text _healthText;
    public Animation death;
    public GameObject hitParticles;



    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode fireKey = KeyCode.Space;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        LoadControls();
    }

    void Update()
    {
        // Move Player
        if (Input.GetKey(leftKey))
        {
            _movement.x = -1;
        }
        else if (Input.GetKey(rightKey))
        {
            _movement.x = 1;
        }
        else
        {
            _movement.x = 0;
        }
        if (Input.GetKey(upKey))
        {
            _movement.y = 1;
        }
        else if (Input.GetKey(downKey))
        {
            _movement.y = -1;
        }
        else
        {
            _movement.y = 0;
        }

        // Check if the player is actually moving
        if (_movement.sqrMagnitude > 0)
        {
            // Calculate the angle of movement in degrees
            float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f;
            // Create a rotation around the Z axis based on the angle
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // Set the player's rotation to the calculated rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }


        if(Input.GetKeyDown(fireKey)){
            Fire();
        }
    }

    void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _movement * _speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            if (_health < 0)
            {
                _health = 0;
                if(death){
                    death.Play("Death");
                }
                Destroy(this, 2.0f);
                
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

    public void IncreasePower(int powerAmount)
    {        
        _power += powerAmount;
        if (_power > _maxPower)
        {
            _power = _maxPower;
        }
    }
    
    public void Fire()
    {
        //TODO: Projectile fire function
        Debug.DrawRay(this.transform.position, this.transform.rotation *Vector3.up*_range, Color.red, 5.0f);
        Debug.Log("Hello");
        this.gameObject.GetComponent<ProjectileWeapon>().Shoot();
    }

    public void LoadControls()
    {
        if (PlayerPrefs.HasKey("upKey"))
        {
            upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey"));
        }

        if (PlayerPrefs.HasKey("downKey"))
        {
            downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey"));
        }

        if (PlayerPrefs.HasKey("leftKey"))
        {
            leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey"));
        }

        if (PlayerPrefs.HasKey("rightKey"))
        {
            rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey"));
        }

        if (PlayerPrefs.HasKey("fireKey"))
        {
            fireKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("fireKey"));
        }
    }
}
