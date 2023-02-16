using UnityEngine;

public class RoomTestPlayer : MonoBehaviour
{
    // THROW THIS ON SOMETHING THAT HAS A RIGIDBODY2D AND A COLLIDER (MAKE SURE IT IS TAGGED PLAYER)
    
    private Rigidbody2D _rb;
    [SerializeField] private float moveSpeed = 20f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        var verticalInput = Input.GetAxisRaw("Vertical");
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var newVelocity = new Vector3(horizontalInput * moveSpeed, verticalInput * moveSpeed);
        _rb.velocity = newVelocity;
    }
}
