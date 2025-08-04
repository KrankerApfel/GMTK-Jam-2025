using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    private bool isJumpPressed;
    private float horizontal;
    public bool IsJumpPressed => isJumpPressed;
    public float Horizontal => horizontal;
    

    void Update()
    {
        if (Sequencer.Instance.isPlaying)
        {
            isJumpPressed = Input.GetButtonDown("Jump");
            horizontal = Input.GetAxis("Horizontal");
        }
        
        // ] is pressed 
        if (Input.GetKeyDown(KeyCode.RightBracket))
            GameManager.Instance.OnLevelFinished();
    
    }
}
