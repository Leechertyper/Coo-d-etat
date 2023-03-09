using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float horizontalTeleportDistance = 25f;
    private const float verticalTeleportDistance = 16f;
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";

    private float tileSize = 3f;
    public float speed = 10f;
    public float dashDistance = 3f;
    private Vector2 _direction;
    private Vector2 _lastMoveDirection;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private bool readyForDash = false;
    private bool dashing = false;
    private Vector2 previousPosition;
    private Vector2 startMovePosition;

    public Animator animator;

    public Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        //transform.up = direction;

        _movement.x = Input.GetAxisRaw(horizontalInput);
        _movement.y = Input.GetAxisRaw(verticalInput);
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Speed", _movement.sqrMagnitude);

        if (!isMoving) // Make sure Player isn't moving

        {
            if (Input.GetKey(KeyCode.W))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.up * tileSize;
                isMoving = true;
                readyForDash = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.down * tileSize;
                isMoving = true;
                readyForDash = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.left * tileSize;
                isMoving = true;
                readyForDash = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.right * tileSize;
                isMoving = true;
                readyForDash = true;
            }
        }
        else if (readyForDash)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                readyForDash = false;
                Vector2 direction = targetPosition - startMovePosition;
                direction.Normalize();
                targetPosition = startMovePosition + direction * tileSize * dashDistance;
                dashing = true;
                animator.SetBool("Dash", true);
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            float moveSpeed = speed;
            if (dashing)
            {
                moveSpeed *= dashDistance;
            } else
            {
                moveSpeed = speed;
            }
            Vector2 currentPosition = rb.position;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // Calculate the movement direction
            Vector2 movementDirection = newPosition - currentPosition;

            // Set the animator parameters
            animator.SetFloat("Horizontal", movementDirection.x * 5);
            animator.SetFloat("Vertical", movementDirection.y * 5);
            animator.SetFloat("Speed", movementDirection.sqrMagnitude * 5);
            animator.SetFloat("PlayerSpeed", speed);
            if ((movementDirection.x <= 0.001 && movementDirection.y <= 0.001) && (_direction.x != 0 || _direction.y != 0))
            {
                _lastMoveDirection = _direction;
            }

            _direction = new Vector2(movementDirection.x, movementDirection.y).normalized;

            animator.SetFloat("LastMoveX", _lastMoveDirection.x);
            animator.SetFloat("LastMoveY", _lastMoveDirection.y);

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
            ResetPlayerState();
            Debug.Log("Rounding Player...");
            RoundPlayerToNearestTile();
        }
    }

    private void RoundPlayerToNearestTile()
    {
        // Move Player to nearest Tile (Accounts for Dash)
        float nearestX = Mathf.Round(transform.position.x + 0.45F);
        float nearestY = Mathf.Round(transform.position.y / tileSize) * tileSize;
        transform.position = new Vector2((nearestX - 0.55f), nearestY);
        Debug.Log("Nearest Tile is " + transform.position);
    }

    private void ResetPlayerState()
    {
        isMoving = false;
        dashing = false;
        readyForDash = false;
        animator.SetBool("Dash", false);
        animator.SetFloat("Speed", 0f);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Wall")) // If collision with "Wall"
        {
            // Get the direction the player is moving in
            Vector2 movementDirection = targetPosition - rb.position;
            movementDirection.Normalize();

            RoundPlayerToNearestTile();

            // Set isMoving to false to allow the player to move again
            ResetPlayerState();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("BottomDoor"))
        {
            StartCoroutine(CheckMovementDelay());
            var newPlayerLocation = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y - verticalTeleportDistance);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;            
        }
        if (col.gameObject.name.Contains("TopDoor"))
        {
            StartCoroutine(CheckMovementDelay());
            var newPlayerLocation = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y + verticalTeleportDistance);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
        if (col.gameObject.name.Contains("LeftDoor"))
        {
            StartCoroutine(CheckMovementDelay());
            var newPlayerLocation = new Vector2(col.gameObject.transform.position.x - horizontalTeleportDistance, col.gameObject.transform.position.y);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
        if (col.gameObject.name.Contains("RightDoor"))
        {
            StartCoroutine(CheckMovementDelay());
            var newPlayerLocation = new Vector2(col.gameObject.transform.position.x + horizontalTeleportDistance, col.gameObject.transform.position.y);
            targetPosition = newPlayerLocation;
            transform.position = newPlayerLocation;
        }
    }
}
