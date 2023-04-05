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

    private float _ammoPerShot = 2;
    private float _lazerSpeed = 25;
    // Input variable for shooting angle
    private float angle;

    // Reference to the projectile prefab to be spawned
    public GameObject projectilePrefab;

    //Damage of the laser
    [SerializeField] private int _damage = 10;

    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyTrail;
    [SerializeField] private Canvas energyCanvas;

    private bool _energyChanging = false;
    private bool _energyTrailChanging = false;
    //[SerializeField] private Text _ammoText;

    // Cooldown time for shooting projectiles
    private float shootTimer = 0f;
    public Vector2 direction;
    public float rotZ;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.inGame)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 rotation = mousePosition - transform.position;
            rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;


            direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            if (_energyChanging)
            {
                energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, _curAmmo / (BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier()), 3f * Time.deltaTime);
                if (Mathf.Round(energyBar.fillAmount * (BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier())) == Mathf.Round(_curAmmo))
                {
                    _energyChanging = false;
                    _energyTrailChanging = true;
                }
                else
                {
                    _energyTrailChanging = false;
                }
            }

            if (_energyTrailChanging)
            {
                energyTrail.fillAmount = Mathf.Lerp(energyTrail.fillAmount, _curAmmo / (BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier()), 5f * Time.deltaTime);
                if (Mathf.Round(energyTrail.fillAmount * (BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier())) == Mathf.Round(_curAmmo))
                {
                    _energyTrailChanging = false;
                }
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                ShootProjectile(direction);
            }

            // Update the shoot timer
            shootTimer -= Time.deltaTime;
        }
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
                projectileRb.AddForce(projectile.transform.up * _lazerSpeed, ForceMode2D.Impulse);
                //projectile.GetComponent<Projectile>().speed;

                // Reset the shoot timer
                shootTimer = BalanceVariables.player["attackSpeed"];
                RemoveAmmo(_ammoPerShot);
               // Debug.Log("All ammo " + _curAmmo);
            }
            else
            {
                shootTimer = BalanceVariables.player["attackSpeed"];
                AkSoundEngine.PostEvent("Play_Machine_Buzz", this.gameObject);
                Debug.Log("NO MORE AMMO");
            }
        }
    }

    private void UpdateAmmoUI()
    {
        energyCanvas.GetComponent<Animator>().SetFloat("energy", _curAmmo);
        _energyChanging = true;
    } 
    public void SetMaxAmmo(float newMaxAmmo)
    {
        UpdateAmmoUI();
        _ammoPerShot = newMaxAmmo;
    }

    public void SetAmmoPerShot(float newAmmoPerShot)
    {
        UpdateAmmoUI();
        _ammoPerShot = newAmmoPerShot;
    }


    public void AddAmmo(float moreAmmo)
    {
        //Debug.Log("The added ammo is " + moreAmmo);
        _curAmmo += moreAmmo;
        if(_curAmmo > BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier())
        {
            _curAmmo = BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier();
        }
        UpdateAmmoUI();
    }

    public void MakeMaxAmmo()
    {
        _curAmmo = BalanceVariables.player["maxAmmo"] * GetComponent<Player>().returnShop().GetBatteryMultiplier();
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