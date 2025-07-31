using UnityEngine;

public class ActionDoubleJump : ActionBase
{
    [Header("Physics")]

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float jumpForce = 15f;

 
    [SerializeField]
    private float speed = 10f;
    
    private float timer = 0f;
    private float jumpInterval = 0.5f;
    private bool has_jump = false;


    public override void HandleAction()
    {
        Debug.Log("ActionDoubleJump");
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0f;
        }
        else if (timer <= jumpInterval && !has_jump) //init phase => first jump
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            has_jump = true;
        }
        else if (timer > jumpInterval && has_jump) //second jump
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            has_jump = false;


        }

    }

}

