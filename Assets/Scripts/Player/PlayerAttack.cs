using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using theNameSpace;
using UnityEngine.UI;
using AK;
using Unity.VisualScripting;

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

    //[SerializeField] private Text _ammoText;

    // Cooldown time for shooting projectiles
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;
    public Vector2 direction;
    public float rotZ;

    



    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);



        //DEBUG Heal func
        if (Input.GetKeyDown(KeyCode.H))
        {
            this.gameObject.GetComponent<Player>().SetHealth(10);
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            ShootProjectile(direction);
            




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
            if(_curAmmo >= _ammoPerShot)
            {
                // Spawn a new projectile at the player's position
                
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0,0,rotZ - 90));
                AkSoundEngine.PostEvent("Play_Lazer_SFX_2", projectile);
                //AkSoundEngine.StopPlayingID(playing, 500, AkCurveInterpolation.AkCurveInterpolation_Constant);

                // Set the velocity of the projectile to the specified direction
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                projectileRb.velocity = direction * 10;
                //projectile.GetComponent<Projectile>().speed;

                // Reset the shoot timer
                shootTimer = shootCooldown;
                RemoveAmmo(_ammoPerShot);
                Debug.Log("All ammo " + _curAmmo);
            }
            else
            {
                Debug.Log("NO MORE AMMO");
            }
        }
    }

    private void UpdateAmmoUI()
    {
        //_ammoText.text =  "Ammo: " + _curAmmo;
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
        UpdateAmmoUI();
    }

    public void MakeMaxAmmo()
    {
        _curAmmo = _maxAmmo;
        UpdateAmmoUI();
    }

    public void RemoveAmmo(float lessAmmo)
    {
        _curAmmo -= lessAmmo;
        if(_curAmmo < 0)
        {
            _curAmmo = 0;
        }
        UpdateAmmoUI();
    }

    public float GetCurrentAmmo()
    {
        return _curAmmo;
    }

}
