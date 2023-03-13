using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using theNameSpace;

public class PlayerAttack : MonoBehaviour
{
    // Input variable for shooting angle
    private float angle;

    // Reference to the projectile prefab to be spawned
    public GameObject projectilePrefab;

    //Damage of the laser
    [SerializeField] private int _damage = 1;

    // Cooldown time for shooting projectiles
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        //DEBUG Heal func
        if (Input.GetKeyDown(KeyCode.H))
        {
            this.gameObject.GetComponent<Player>().SetHealth(10);
        }
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

    // Method to shoot a projectile in the specified direction
    private void ShootProjectile(Vector2 direction)
    {
        // Check if the shoot timer has expired
        if (shootTimer <= 0f)
        {
            // Spawn a new projectile at the player's position
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            // Set the velocity of the projectile to the specified direction
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * 10;
            //projectile.GetComponent<Projectile>().speed;

            // Reset the shoot timer
            shootTimer = shootCooldown;
        }
    }
}
