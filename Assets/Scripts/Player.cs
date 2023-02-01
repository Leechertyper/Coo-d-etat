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
    private Rigidbody2D _rigidBody;
    private Vector2 _movement;
    [SerializeField] private Text _healthText;

    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw(horizontalInput);
        _movement.y = Input.GetAxisRaw(verticalInput);

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


        // TEST CODE Decrease the player's health by 1 when the z key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(1);
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
            }
            UpdateHealthUI();
        }        
    }

    private void UpdateHealthUI()
    {
        _healthText.text = "HP " + _health + "/" + _maxHealth;
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
}
