using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField]
    private float jumpForce = 30f;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private BoxCollider2D foots;
    [SerializeField]
    private float collisionRadius = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    
    private Rigidbody2D rigidBody;
    private PlayerInputs inputs;
    private bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        inputs = GetComponent<PlayerInputs>();
    }

    public void addForce(Vector2 velocity) 
    {
        rigidBody.AddRelativeForce(velocity);
    }

    private void Update()
    {
        CheckGround();
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        Vector2 velocity = rigidBody.linearVelocity;
        velocity.x = speed * inputs.Horizontal;
        rigidBody.linearVelocity = velocity;
    }

    private void HandleJump() 
    {
        if(inputs.IsJumpPressed && isGrounded) 
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        }
    }
    private void CheckGround() 
    {
        isGrounded = Physics2D.OverlapCircle(foots.transform.position, collisionRadius, groundLayer);
    }
}
