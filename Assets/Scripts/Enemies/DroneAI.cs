using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Changable values for balance
 *  
 *  float moveSpeed - The speed of the Enemy
 *  float range - The range when the enemy will stop chaseing you
 *  float pauseTime - The time before the enemy will continue to chase you
 */
public class DroneAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float range = 3f;
    public float pauseTime = 3;
    [Header("Attack")]
    public float slowShotCD = 1;
    public float fastShotCD = 0.5f;
    public float angle;
    [Header("Components")]
    public ProjectileWeapon wp;
    public Health hp;
    public SpriteManager sprite;

    private IEnumerator _slowFire, _fastFire;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private Transform _target;
    private state _myState;
    enum state {Chase, Pause};

    // Start is called before the first frame update
    void Start()
    {
        _myState = state.Chase;
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _slowFire = TimedShoot(slowShotCD);
        _fastFire = TimedShoot(fastShotCD);
        StartCoroutine(_slowFire);
    }

    // Update is called once per frame
    void Update()
    {
        LookAt();
    }

    private void FixedUpdate()
    {
        if(_myState == state.Chase)
        {
            if (_target)
            {
                Move(_moveDirection);
            }
        }
        if(Vector2.Distance(_target.position, transform.position) < range && _myState == state.Chase)
        {
            _myState = state.Pause;
            _rb.velocity = Vector2.zero;
            StopCoroutine(_slowFire);
            StartCoroutine(_fastFire);
            Invoke("resumeChase", pauseTime);
        }

    }

    /// <summary>
    /// Contunies to chase target if out of range
    /// If in range invoke this function in 1 second (Recursive)
    /// </summary>
    /// Note* Recursive function, will call itself untill it can chase again
    private void resumeChase()
    {
        if (!(Vector2.Distance(_target.position, transform.position) < range))
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
            if(direction.y >= 0.4)
            {
                sprite.Down();
            }
            else if (direction.y <= -0.4)
            {
                sprite.Up();
            }
            else
            {
                sprite.Side();
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
        _rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
    }

    /// <summary>
    /// Fires shoots from attached weapon every "delay" seconds
    /// </summary>
    /// <param name="delay">The time you want between shots</param>
    /// <returns>IEnum Wait</returns>
    private IEnumerator TimedShoot(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            wp.Shoot(angle);
        }
    }

    /// <summary>
    /// Stops all action in this script for death
    /// </summary>
    public void Die()
    {
        _rb.velocity = Vector2.zero;
        StopAllCoroutines();
        this.enabled = false;
    }

    public void Awaken()
    {
        _myState = state.Chase;
        StartCoroutine(_slowFire);
    }

    public void Sleep()
    {
        StopAllCoroutines();
        _myState = state.Pause;
        _rb.velocity = Vector2.zero;
    }


}
