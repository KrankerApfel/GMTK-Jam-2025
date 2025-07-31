using UnityEngine;
public class ActionSimpleJump : ActionBase
{
    [Header("Physics")]

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float jumpForce = 30f;
    
    private float timer = 0f;
    private bool has_jump = false;


    public override void HandleAction()
    {
        Debug.Log("ActionDoubleJump");
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0f;
            has_jump = false;
        }
        else if (timer < duration && !has_jump) //init phase => first jump
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            has_jump = true;
        }


    }

}

