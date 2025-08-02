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

    private float jumpInterval = 0.3f;

    [SerializeField] private bool disableMovementDuringJump = true;

    public override void Init()
    {
        playerPhysics = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPhysics>();
        rigidBody = playerPhysics.gameObject.GetComponent<Rigidbody2D>();
    }

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
        StartCoroutine(doubleJump());
        OnActionFinished?.Invoke();
    }

}

