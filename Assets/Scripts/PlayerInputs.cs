using System;
using TMPro;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private bool isJumpPressed;
    private float horizontal;
    private Vector3 mousePosition;
    private TextMeshPro cross;
    private LineRenderer lineRenderer;
    
    [SerializeField]
    private GameObject bulletPrefab;
    
    public float ShootingRange = 5f;
    public float BulletSpeed = 10f;
    public bool IsJumpPressed => isJumpPressed;
    public float Horizontal => horizontal;

    private void Start()
    {
        cross = GetComponentInChildren<TextMeshPro>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        if (Sequencer.Instance.isPlaying)
        {
            isJumpPressed = Input.GetButtonDown("Jump");
            horizontal = Input.GetAxis("Horizontal");
            
            Vector3 mouse3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mouse3D.x, mouse3D.y, 0f);
            cross.transform.position = transform.position + (mousePosition - transform.position).normalized * ShootingRange;
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, cross.transform.position);
            lineRenderer.useWorldSpace = true;
            
            //draw line from player to aim
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = (cross.transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.transform.up = direction; // Set the bullet's rotation to face the aim direction
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * BulletSpeed;
            }
        }


    }
}
