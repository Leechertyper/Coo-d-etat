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
public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float range = 3f;
    public float pauseTime = 3;
    public Weapon weapon;
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
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _rb.rotation = angle;
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
}
