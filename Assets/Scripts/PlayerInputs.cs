using System;
using TMPro;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private bool isJumpPressed;
    private float horizontal;
    private Vector3 mousePosition;
    private TextMeshPro aim;
    
    public float ShootingRange = 5f;
    public bool IsJumpPressed => isJumpPressed;
    public float Horizontal => horizontal;

    private void Start()
    {
        aim = GetComponentInChildren<TextMeshPro>();
    }

    void Update()
    {
        if (Sequencer.Instance.isPlaying)
        {
            isJumpPressed = Input.GetButtonDown("Jump");
            horizontal = Input.GetAxis("Horizontal");
            
            Vector3 mouse3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mouse3D.x, mouse3D.y, 0f);
            aim.transform.position = transform.position + (mousePosition - transform.position).normalized * ShootingRange;
            
            //draw line from player to aim
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse button clicked");
            }
        }


    }
}
