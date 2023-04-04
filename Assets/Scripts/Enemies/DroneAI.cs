using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Changable values for balance
 *  
 *  float BalanceVariables.droneEnemy["moveSpeed"] - The speed of the Enemy
 *  float BalanceVariables.droneEnemy["range"] - The BalanceVariables.droneEnemy["range"] when the enemy will stop chaseing you
 *  float BalanceVariables.droneEnemy["pauseTime"] - The time before the enemy will continue to chase you
 */
public class DroneAI : Enemy
{
    public float angle;
    [Header("Components")]
    public ProjectileWeapon wp;
    public Health hp;
    public Animator animator;

    private IEnumerator _slowFire, _fastFire;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private Transform _target;
    private state _myState;
    private bool _awake;
    private bool _dead = false;
    enum state {Chase, Pause};

    private bool _isSleeping;
    
    // Start is called before the first frame update
    void Start()
    {
        _awake = false;
        _myState = state.Chase;
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _slowFire = TimedShoot(BalanceVariables.droneEnemy["slowShotCD"]);
        _fastFire = TimedShoot(BalanceVariables.droneEnemy["fastShotCD"]);
        
        BalanceVariables.seenDictionaries["droneEnemy"] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_awake)
        {
            LookAt();
        }
    }

    private void FixedUpdate()
    {
        if (_awake)
        {
            if(_myState == state.Chase)
            {
                if (_target)
                {
                    Move(_moveDirection);
                }
            }
            if(Vector2.Distance(_target.position, transform.position) < BalanceVariables.droneEnemy["range"] && _myState == state.Chase)
            {
                _myState = state.Pause;
                _rb.velocity = Vector2.zero;
                StopCoroutine(_slowFire);
                StartCoroutine(_fastFire);
                Invoke("resumeChase", BalanceVariables.droneEnemy["pauseTime"]);
            }
        }
    }

    /// <summary>
    /// Contunies to chase target if out of BalanceVariables.droneEnemy["range"]
    /// If in BalanceVariables.droneEnemy["range"] invoke this function in 1 second (Recursive)
    /// </summary>
    /// Note* Recursive function, will call itself untill it can chase again
    private void resumeChase()
    {
        if (!(Vector2.Distance(_target.position, transform.position) < BalanceVariables.droneEnemy["range"]))
        {
            _myState = state.Chase;
            StopCoroutine(_fastFire);
            StartCoroutine(_slowFire);
        }
        else
        {
            Invoke("resumeChase", 1);
        }
    }

    /// <summary>
    /// Rotate toward target and set direction
    /// </summary>
    private void LookAt()
    {
        if (_target)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            if(direction.y >= 0.2)
            {
                animator.SetBool("IsSide", true);
            }
            else if (direction.y <= -0.2)
            {
                animator.SetBool("IsSide", true);
            }
            else
            {
                animator.SetBool("IsSide", false);
            }
            _moveDirection = direction;
        }
    }

    /// <summary>
    /// Adds velociry to the enemy in direction
    /// </summary>
    /// <param name="direction"> Vector2 direction for enemy to move</param>
    private void Move(Vector2 direction)
    {
        _rb.velocity = new Vector2(direction.x, direction.y) * BalanceVariables.droneEnemy["moveSpeed"];
    }

    /// <summary>
    /// Fires shoots from attached weapon every "delay" seconds
    /// </summary>
    /// <param name="delay">The time you want between shots</param>
    /// <returns>IEnum Wait</returns>
    private IEnumerator TimedShoot(float delay)
    {
        while (!_isSleeping)
        {
            yield return new WaitForSeconds(delay);
            AkSoundEngine.PostEvent("Play_Lazer_SFX", this.gameObject);
            wp.Shoot(angle);
        }
    }

    /// <summary>
    /// Stops all action in this script for death
    /// </summary>
    public override void Die()
    {
        if(_dead==false)
        {
            GameObject.Find("ScoreManager").GetComponent<Score>().AddScore(100);
            _dead = true;
        }
        _rb.velocity = Vector2.zero;
        StopAllCoroutines();
        //AkSoundEngine.PostEvent("Play_Robot_Ouch", this.gameObject);
        this.enabled = false;

    }

    public override void TakeDamage()
    {
        AkSoundEngine.PostEvent("Play_Robot_Ouch", this.gameObject);
    }

    public override void Awaken()
    {
        AkSoundEngine.PostEvent("Play_Robot_Alert_SFX", this.gameObject);
        _awake = true;
        _myState = state.Chase;
        _isSleeping = false;
        StartCoroutine(_slowFire);
        
    }

    public override void Sleep()
    {
        _awake = false;
        _myState = state.Pause;
        _rb.velocity = Vector2.zero;
        _isSleeping = true;
        StopAllCoroutines();
    }

    public void ChangeMoveSpeed(float newMoveSpeed)
    {
        BalanceVariables.droneEnemy["moveSpeed"] = newMoveSpeed;
    }

    public void ChangeAttackSpeed(float newAttackSpeed)
    {
        
        BalanceVariables.droneEnemy["slowShotCD"] = newAttackSpeed;

        BalanceVariables.droneEnemy["fastShotCD"] = newAttackSpeed/2; // maybe there needs to be another fast shot var?
    }

    public override float GetHealthVariable()
    {
        return BalanceVariables .droneEnemy["maxHealth"];
    }

}
