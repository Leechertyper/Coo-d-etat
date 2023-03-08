using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float horizontalTeleportDistance = 24f;
    private const float verticalTeleportDistance = 15f;
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";

    private float tileSize = 3f;
    public float speed = 10f;
    private Vector2 _direction;
    private Vector2 _lastMoveDirection;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Vector2 previousPosition;
    private Vector2 startMovePosition;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isMoving) // Make sure Player isn't moving
        {
            if (Input.GetKey(KeyCode.W))
            {
                startMovePosition = rb.position;
                targetPosition = rb.position + Vector2.up * tileSize;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                startMovePosition = rb.position;
                targetPosition = rb.position + Vector2.down * tileSize;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                startMovePosition = rb.position;
                targetPosition = rb.position + Vector2.left * tileSize;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                startMovePosition = rb.position;
                targetPosition = rb.position + Vector2.right * tileSize;
                isMoving = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 currentPosition = rb.position;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);
            Debug.Log("Target Position = " + newPosition);
            rb.MovePosition(newPosition);

            // Calculate the movement direction
            Vector2 movementDirection = newPosition - currentPosition;

            // Set the animator parameters
            animator.SetFloat("Horizontal", movementDirection.x * 5);
            animator.SetFloat("Vertical", movementDirection.y * 5);
            animator.SetFloat("Speed", movementDirection.sqrMagnitude);
            if ((movementDirection.x <= 0.001 && movementDirection.y <= 0.001) && (_direction.x != 0 || _direction.y != 0))
            {
                _lastMoveDirection = _direction;
            }

            _direction = new Vector2(movementDirection.x, movementDirection.y).normalized;

            animator.SetFloat("LastMoveX", _lastMoveDirection.x);
            animator.SetFloat("LastMoveY", _lastMoveDirection.y);

            Debug.Log(movementDirection.y);
            // check if the player has stopped moving
            StartCoroutine(CheckMovementDelay());

            previousPosition = rb.position;
        }
    }

    IEnumerator CheckMovementDelay()
    {
        yield return new WaitForSeconds(0.01f); // Wait for 0.001 seconds after teleportation

        // Check if no player movement
        if (Vector2.Distance(previousPosition, rb.position) < 0.0001f)
        {
            isMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Wall")) // If collision with "Wall"
        {
            // Get the direction the player is moving in
            Vector2 movementDirection = targetPosition - rb.position;
            movementDirection.Normalize();

            // Move the player back to its previous position
            transform.position = startMovePosition;

            // Set isMoving to false to allow the player to move again
            isMoving = false;
            animator.SetFloat("Speed", 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("BottomDoor"))
        {
            var newPlayerLocation = new Vector2(targetPosition.x, targetPosition.y - verticalTeleportDistance);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
        if (col.gameObject.name.Contains("TopDoor"))
        {
            var newPlayerLocation = new Vector2(targetPosition.x, targetPosition.y + verticalTeleportDistance);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
        if (col.gameObject.name.Contains("LeftDoor"))
        {
            var newPlayerLocation = new Vector2(targetPosition.x - horizontalTeleportDistance, targetPosition.y);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
        if (col.gameObject.name.Contains("RightDoor"))
        {
            var newPlayerLocation = new Vector2(targetPosition.x + horizontalTeleportDistance, targetPosition.y);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
    }
}
