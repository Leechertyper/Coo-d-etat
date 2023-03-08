using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed of the player
    public float moveSpeed = 6f;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Input values for horizontal and vertical movement
    private Vector2 _movement;
    private Vector2 _movementDirection;
    private Vector2 _lastMoveDirection;    

    
    private float _rotationSpeed = 15f;

    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";

    public Animator animator;
    public Vector2 direction;
    void Start()
    {
        // Get the reference to the Rigidbody component on this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        transform.up = direction;

        _movement.x = Input.GetAxisRaw(horizontalInput);
        _movement.y = Input.GetAxisRaw(verticalInput);
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Speed", _movement.sqrMagnitude);

        if ((_movement.x == 0 && _movement.y == 0) && (_movementDirection.x != 0 || _movementDirection.y != 0))
        {
            _lastMoveDirection = _movementDirection;
        }

        //_movementDirection = new Vector2(_movement.x, _movement.y).normalized;
        _movementDirection = new Vector2(direction.x,direction.y).normalized;

        animator.SetFloat("LastMoveX", _lastMoveDirection.x);
        animator.SetFloat("LastMoveY", _lastMoveDirection.y);

        
    }

    void FixedUpdate()
    {
        // Calculate the movement vector based on the input values and the movement speed
        Vector2 movement = new Vector2(_movement.x, _movement.y) * moveSpeed;

        // Apply the movement vector to the Rigidbody component
        rb.velocity = movement;
        
 
    }

    
}
