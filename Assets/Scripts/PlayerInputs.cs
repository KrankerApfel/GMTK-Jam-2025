using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private bool isJumpPressed;
    private float horizontal;
    public bool IsJumpPressed => isJumpPressed;
    public float Horizontal => horizontal; 
    void Update()
    {
        isJumpPressed = Input.GetButtonDown("Jump");
        horizontal = Input.GetAxis("Horizontal");
    }
}
