using UnityEngine;
using System.Collections;
public class ActionDoubleJump : ActionBase
{
    [Header("Physics")]

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float jumpForce = 25f;
    
    private float timer = 0f;
    private float jumpInterval = 0.3f;
    private bool has_jump = false;

    public void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    public IEnumerator waitUntilSecondJump()
    {
                Debug.Log("start");
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        yield return new WaitForSeconds(jumpInterval);
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        has_jump = true;

        Debug.Log("end");

    }


    public override void HandleAction()
    {
        Debug.Log("ActionDoubleJump");
        Debug.Log(timer);

        if (timer >= duration)
        {
            timer = 0f;
            has_jump = false;

        }
        Debug.Log(timer);

        if (timer <= jumpInterval && !has_jump) //init phase => first jump
        {
            StartCoroutine(waitUntilSecondJump());

        }

    }

}

