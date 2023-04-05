using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private const float horizontalTeleportDistance = 27f;
    private const float verticalTeleportDistance = 18f;
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

    //public GlobalGrid theGrid = GameManager.Instance.Grid.GetComponent<GlobalGrid>();

    public Vector2Int startInt;

    public Vector2Int curPlace;

    private float lastDashTime;
    public float dashCooldown = 2f; // dash cooldown duration in seconds
    public Slider dashSlider;

    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode dashKey = KeyCode.Space;

    void Start()
    {
        /*transform.position = GameManager.Instance.Grid.GetTile(transform.position);*/
        rb = GetComponent<Rigidbody2D>();
        startInt = new Vector2Int(8,4);
        LoadControls();
    }

    void Update()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        //transform.up = direction;

        /*_movement.x = Input.GetAxisRaw(horizontalInput);
        _movement.y = Input.GetAxisRaw(verticalInput);
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Speed", _movement.sqrMagnitude);*/

        if (!isMoving) // Make sure Player isn't moving

        {
            if (Input.GetKey(upKey))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.up * tileSize;
                isMoving = true;
                readyForDash = true;
            }
            else if (Input.GetKey(downKey))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.down * tileSize;
                isMoving = true;
                readyForDash = true;
            }
            else if (Input.GetKey(leftKey))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.left * tileSize;
                isMoving = true;
                readyForDash = true;
            }
            else if (Input.GetKey(rightKey))
            {
                startMovePosition = rb.position;
                targetPosition = startMovePosition + Vector2.right * tileSize;
                isMoving = true;
                readyForDash = true;
            }
        }
        else if (readyForDash && Time.time - lastDashTime > dashCooldown)
        {
            if (Input.GetKey(dashKey))
            {
                lastDashTime = Time.time;
                readyForDash = false;
                Vector2 direction = targetPosition - startMovePosition;
                direction.Normalize();
                targetPosition = startMovePosition + direction * tileSize * dashDistance;
                dashing = true;
                AkSoundEngine.PostEvent("Play_Pigeon_wing_flutter", this.gameObject);
                animator.SetBool("Dash", true);
                StartCoroutine(InvincibilityFrame());
            }
        }

        // Set slider value to the fraction of time left until the next dash
        dashSlider.value = 1f - (Time.time - lastDashTime) / dashCooldown;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Debug.Log("MOVING");
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
            //theGrid.MoveToTile(this.gameObject,new Vector2Int(),newPosition);
            // Calculate the movement direction
            Vector2 movementDirection = newPosition - currentPosition;

            //theGrid.MoveToTile(this.gameObject, )
           // theGrid.MoveToTile(this.gameObject,curPlace,curPlace + new Vector2Int(0,1));
            curPlace += new Vector2Int(0,1);
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

    public void MoveTostart(List<int> theValues)
    {
        //transform.position = GameManager.Instance.Grid.GetTile(startInt);
        rb.MovePosition(new Vector2(0,-4.5f));
        transform.position = new Vector2(0,-4.5f);
        //transform.position = new Vector2(theValues[0],theValues[1]);
        isMoving = false;

    }

    IEnumerator CheckMovementDelay()
    {
        yield return new WaitForSeconds(0.01f); // Wait for 0.001 seconds after teleportation

        // Check if no player movement
        if (Vector2.Distance(previousPosition, rb.position) < 0.0001f)
        {
            ResetPlayerState();
            //Debug.Log("Rounding Player...");
            RoundPlayerToNearestTile();
        }
    }

    private void RoundPlayerToNearestTile()
    {

        transform.position = GameManager.Instance.Grid.GetTile(transform.position);
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

    public void BottomDoor()
    {
        StartCoroutine(CheckMovementDelay());
        targetPosition = new Vector2(transform.position.x, transform.position.y - verticalTeleportDistance);
        transform.position = targetPosition;
    }
    public void TopDoor()
    {
        StartCoroutine(CheckMovementDelay());
        targetPosition = new Vector2(transform.position.x, transform.position.y + verticalTeleportDistance);
        transform.position = targetPosition;
    }
    public void LeftDoor()
    {
        StartCoroutine(CheckMovementDelay());
        targetPosition = new Vector2(transform.position.x - horizontalTeleportDistance, transform.position.y);
        transform.position = targetPosition;
    }
    public void RightDoor()
    {
        StartCoroutine(CheckMovementDelay());
        targetPosition = new Vector2(transform.position.x + horizontalTeleportDistance, transform.position.y);
        transform.position = targetPosition;
    }

    public void LoadControls()
    {
        if (PlayerPrefs.HasKey("upKey"))
        {
            upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey"));
        }

        if (PlayerPrefs.HasKey("downKey"))
        {
            downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey"));
        }

        if (PlayerPrefs.HasKey("leftKey"))
        {
            leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey"));
        }

        if (PlayerPrefs.HasKey("rightKey"))
        {
            rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey"));
        }

        if (PlayerPrefs.HasKey("dashKey"))
        {
            dashKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("dashKey"));
        }
    }

    /// <summary>
    /// Makes the Pigeon invulnerable during the duration of the dash animation
    /// </summary>
    /// <returns>N/A</returns>
    private IEnumerator InvincibilityFrame()
    {
        var playerScript = GetComponent<Player>();
        playerScript.iFrame = true;
        yield return new WaitForSeconds(0.267f);
        playerScript.iFrame = false;
    }
}
