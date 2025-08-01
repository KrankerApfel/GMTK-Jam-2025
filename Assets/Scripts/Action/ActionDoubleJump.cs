using UnityEngine;
using System.Collections;
public class ActionDoubleJump : ActionBase
{
    [Header("Physics")]

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField] private PlayerPhysics playerPhysics;

    [SerializeField]
    private float jumpForce = 25f;
    
    private float timer = 0f;
    private float jumpInterval = 0.3f;
    private bool has_jump = false;

    [SerializeField] private bool disableMovementDuringJump = true;


    public IEnumerator doubleJump()
    {
        if (disableMovementDuringJump)
        {
            playerPhysics.DisableMovement(true);
        }

        rigidBody.linearVelocity = new Vector2(playerPhysics.Velocity.x, jumpForce);


        yield return new WaitForSeconds(jumpInterval);
        rigidBody.linearVelocity = new Vector2(playerPhysics.Velocity.x, jumpForce);

        yield return new WaitForSeconds(duration - jumpInterval);

        if (disableMovementDuringJump)
        {
            playerPhysics.DisableMovement(false);
        }


    }


    public override void HandleAction()
    {
        Debug.Log("ActionDoubleJump");

        StartCoroutine(doubleJump());
        OnActionFinished?.Invoke();


    }

}

