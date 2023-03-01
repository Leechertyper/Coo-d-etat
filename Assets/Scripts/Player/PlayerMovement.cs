using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float horizontalTeleportDistance = 27f;
    private const float verticalTeleportDistance = 18f;
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";

    private float tileSize = 3f;
    public float speed = 10f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Vector2 previousPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isMoving) // Make sure Player isn't moving
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                targetPosition = rb.position + Vector2.up * tileSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetPosition = rb.position + Vector2.down * tileSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                targetPosition = rb.position + Vector2.left * tileSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
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
            rb.MovePosition(newPosition);

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
