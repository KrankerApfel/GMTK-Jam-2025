using System;
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

    public Vector2 Velocity => rigidBody.linearVelocity;

    public Action OnPlayerDestroyed;

    private Rigidbody2D rigidBody;
    private PlayerInputs inputs;
    private bool isGrounded;
    private bool disableMovement;

    private ContactFilter2D contactFilter;
    private Collider2D[] contacts = new Collider2D[10]; // Adjust size as needed


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        inputs = GetComponent<PlayerInputs>();

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(groundLayer);
        contactFilter.useLayerMask = true;
    }


    public void DisableMovement(bool deactivate)
    {
        disableMovement = deactivate;
    }

    public void SetVelocity(Vector2 velocity) 
    {
        rigidBody.linearVelocity = velocity;
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
        if (disableMovement) return;
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
        if(foots == null) Destroy(gameObject);
        int contactCount = foots.GetContacts(contactFilter, contacts);
        isGrounded = contactCount > 0;
        //isGrounded = Physics2D.OverlapCircle(foots.transform.position, collisionRadius, groundLayer);
    }

    private void OnDestroy()
    {
        OnPlayerDestroyed?.Invoke();
    }
}
