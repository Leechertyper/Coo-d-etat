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
    private Vector2 _movementDirection;
    private Vector2 _lastMoveDirection;
    // Input variable for shooting angle
    private float angle;

    // Reference to the projectile prefab to be spawned
    public GameObject projectilePrefab;

    //Damage of the laser
    [SerializeField] private int _damage;

    // Cooldown time for shooting projectiles
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;
    
    private float _rotationSpeed = 15f;

    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";

    public Animator animator;
    void Start()
    {
        // Get the reference to the Rigidbody component on this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        _movement.x = Input.GetAxisRaw(horizontalInput);
        _movement.y = Input.GetAxisRaw(verticalInput);
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Speed", _movement.sqrMagnitude);

        if ((_movement.x == 0 && _movement.y == 0) && (_movementDirection.x != 0 || _movementDirection.y != 0))
        {
            _lastMoveDirection = _movementDirection;
        }

        _movementDirection = new Vector2(_movement.x, _movement.y).normalized;

        animator.SetFloat("LastMoveX", _lastMoveDirection.x);
        animator.SetFloat("LastMoveY", _lastMoveDirection.y);

        // Check if the arrow keys are being pressed, and shoot projectiles in the corresponding direction
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            angle = 0f;
            ShootProjectile(Vector2.up);
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            angle = 180f;
            ShootProjectile(Vector2.down);
            
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            angle = 90f;
            ShootProjectile(Vector2.left);
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            angle = -90f;
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
        
 
    }

    // Method to shoot a projectile in the specified direction
    void ShootProjectile(Vector2 direction)
    {
        // Check if the shoot timer has expired
        if (shootTimer <= 0f)
        {
            // Spawn a new projectile at the player's position
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0,0,angle));
            // Set the damage and tag of the projectile
            projectile.GetComponent<Lazer>().SetPower(_damage);
            projectile.gameObject.tag = "PlayerProjectile";
            // Set the velocity of the projectile to the specified direction
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * 10;
            //projectile.GetComponent<Projectile>().speed;

            // Reset the shoot timer
            shootTimer = shootCooldown;
        }
    }
}
