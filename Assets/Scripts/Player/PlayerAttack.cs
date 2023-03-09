using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using theNameSpace;

public class PlayerAttack : MonoBehaviour
{

    //For the players ammo
    private float _curAmmo = 100;

    private float _maxAmmo = 100;

    private float _ammoPerShot = 2;
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
    void ShootProjectile(Vector2 direction)
    {
        // Check if the shoot timer has expired
        if (shootTimer <= 0f)
        {
            if(_curAmmo >= _ammoPerShot)
            {
                // Spawn a new projectile at the player's position
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
                // Set the damage and tag of the projectile
                //projectile.GetComponent<Lazer>().SetPower(_damage);
                projectile.GetComponent<Lazer>().setParentType(theNameSpace.TheParentTypes.playerType);
                projectile.gameObject.tag = "PlayerProjectile";
                // Set the velocity of the projectile to the specified direction
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                projectileRb.velocity = direction * 10;
                //projectile.GetComponent<Projectile>().speed;

                // Reset the shoot timer
                shootTimer = shootCooldown;
                RemoveAmmo(_ammoPerShot);
                //Debug.Log("All ammo " + _curAmmo);
            }
            else
            {
                Debug.Log("NO MORE AMMO");
            }
        }
    }

    public void SetMaxAmmo(float newMaxAmmo)
    {
        _ammoPerShot = newMaxAmmo;
    }

    public void SetAmmoPerShot(float newAmmoPerShot)
    {
        _ammoPerShot = newAmmoPerShot;
    }


    public void AddAmmo(float moreAmmo)
    {
        //Debug.Log("The added ammo is " + moreAmmo);
        _curAmmo += moreAmmo;
        if(_curAmmo > _maxAmmo)
        {
            _curAmmo = _maxAmmo;
        }
    }
 
    public void RemoveAmmo(float lessAmmo)
    {
        _curAmmo -= lessAmmo;
        if(_curAmmo < 0)
        {
            _curAmmo = 0;
        }
    }

    public float GetCurrentAmmo()
    {
        return _curAmmo;
    }

}
