using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed of the player
    public float moveSpeed = 5f;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Input values for horizontal and vertical movement
    private Vector2 _movement;

    // Reference to the projectile prefab to be spawned
    public GameObject projectilePrefab;

    // Cooldown time for shooting projectiles
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;
    
    private float _rotationSpeed = 15f;

    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";
    
    void Start()
    {
        // Get the reference to the Rigidbody component on this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        _movement.x = Input.GetAxisRaw(horizontalInput);
        _movement.y = Input.GetAxisRaw(verticalInput);

        // Check if the player is actually moving
        //if (_movement.sqrMagnitude > 0)
        //{
        //    var direction = _movement;
            
            // Calculate the angle of movement in degrees
            //float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f;
            
            
            // Create a rotation around the Z axis based on the angle
            //Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            
            // Set the player's rotation to the calculated rotation
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            //transform.up = direction;
        //}
        // Get the input values for horizontal and vertical movement
        // moveHorizontal = Input.GetAxis("Horizontal");
        // moveVertical = Input.GetAxis("Vertical");

        // Check if the arrow keys are being pressed, and shoot projectiles in the corresponding direction
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShootProjectile(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShootProjectile(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShootProjectile(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShootProjectile(Vector2.right);
        }

        // Update the shoot timer
        shootTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        // Calculate the movement vector based on the input values and the movement speed
        Vector2 movement = new Vector2(_movement.x, _movement.y) * moveSpeed;

        // Apply the movement vector to the Rigidbody component
        rb.velocity = movement;
        
        if (_movement.x > 0)
        {
            transform.up = Vector2.right; // Face right
        }
        else if (_movement.x < 0)
        {
            transform.up = Vector2.left; // Face left
        }
        else if (_movement.y > 0)
        {
            transform.up = Vector2.up; // Face up
        }
        else if (_movement.y < 0)
        {
            transform.up = Vector2.down; // Face down
        }
    }

    // Method to shoot a projectile in the specified direction
    void ShootProjectile(Vector2 direction)
    {
        // Check if the shoot timer has expired
        if (shootTimer <= 0f)
        {
            // Spawn a new projectile at the player's position
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Set the velocity of the projectile to the specified direction
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * 10;
            //projectile.GetComponent<Projectile>().speed;

            // Reset the shoot timer
            shootTimer = shootCooldown;
        }
    }
}
