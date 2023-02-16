using UnityEngine;

public class RoomTestPlayer : MonoBehaviour
{
    // THROW THIS ON SOMETHING THAT HAS A RIGIDBODY2D AND A COLLIDER (MAKE SURE IT IS TAGGED PLAYER)
    
    private Rigidbody2D _rb;
    private float _verticalInput;
    private float _horizontalInput;
    [SerializeField] private float moveSpeed = 20f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void Update()
    {
        var newVelocity = new Vector3(_horizontalInput * moveSpeed, _verticalInput * moveSpeed);
        _rb.velocity = newVelocity;
    }
}
