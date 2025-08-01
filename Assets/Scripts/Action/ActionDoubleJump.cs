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



    public void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    public IEnumerator waitUntilSecondJump()
    {
        if (disableMovementDuringJump)
        {
            playerPhysics.DisableMovement(true);
        }

        rigidBody.linearVelocity = new Vector2(playerPhysics.Velocity.x, jumpForce);


        yield return new WaitForSeconds(jumpInterval);
        rigidBody.linearVelocity = new Vector2(playerPhysics.Velocity.x, jumpForce);
        has_jump = true;
    }


    public override void HandleAction()
    {
        Debug.Log("ActionDoubleJump");
        Debug.Log(timer);

        if (timer >= duration)
        {     if (disableMovementDuringJump)
            {
                playerPhysics.DisableMovement(false);
            }
            timer = 0f;
            has_jump = false;
            OnActionFinished?.Invoke();
        }
        Debug.Log(timer);

        if (timer <= jumpInterval && !has_jump) //init phase => first jump
        {
            StartCoroutine(waitUntilSecondJump());
    



        }

    }

}

