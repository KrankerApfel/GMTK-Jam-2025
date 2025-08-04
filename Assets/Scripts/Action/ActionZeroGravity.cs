using System.Collections;
using UnityEngine;
public class ActionZeroGravity : ActionBase
{
    private PlayerPhysics playerPhysics;
    private Rigidbody2D playerRigidbody;
    [SerializeField] private bool disableMovementDuringJump = true;
    
    public float damping = 3f; // Damping value to apply when in zero gravity
    private float originalGravityScale; // Original gravity scale to restore after action
    private float originalLinearDamping; // Original linear damping to restore after action
    private float originalAngularDamping;
    


    public override void Init()
    {
        playerRigidbody = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        playerPhysics = GameManager.Instance.player.GetComponent<PlayerPhysics>();
        
        originalGravityScale = playerRigidbody.gravityScale;
        originalLinearDamping = playerRigidbody.linearDamping;
        originalAngularDamping = playerRigidbody.angularDamping;
    }

    public override void HandleAction()
    {
        playerRigidbody.gravityScale = 0f; // Disable gravity
        playerRigidbody.linearDamping = damping; // Apply damping to linear velocity
        if (disableMovementDuringJump)
        {
            playerPhysics.DisableMovement(true);
        }
    }
    
    public override void PreAction() { return; }

    public override void PostAction()
    {
        playerRigidbody.gravityScale = originalGravityScale; // Restore original gravity scale
        playerRigidbody.linearDamping = originalLinearDamping; // Restore original linear damping
        if (disableMovementDuringJump)
        {
            playerPhysics.DisableMovement(false);
        }
    }
}