using UnityEngine;
using System.Collections;

public class ActionSimpleJump : ActionBase
{
    [Header("Physics")]

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float jumpForce = 15f;



    public override void HandleAction()
    {
        Debug.Log("ActionSimpleJump");

        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
          


    }

}

