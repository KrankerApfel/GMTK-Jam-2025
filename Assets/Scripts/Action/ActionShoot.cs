using System.Collections;
using TMPro;
using UnityEngine;
public class ActionShoot : ActionBase
{
    private Vector3 mousePosition;
    private TextMeshPro cross;
    private LineRenderer lineRenderer;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float ShootingRange = 5f;
    [SerializeField] private float BulletSpeed = 10f;

    private void Start()
    {
        cross = GetComponentInChildren<TextMeshPro>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }
    
    public override void PreAction()
    {
        Vector3 mouse3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mouse3D.x, mouse3D.y, 0f);
        cross.transform.position = transform.position + (mousePosition - transform.position).normalized * ShootingRange;
            
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, cross.transform.position);
        lineRenderer.useWorldSpace = true;
    }
    
    public override void HandleAction()
    {
        //draw line from player to aim
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 direction = (cross.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.up = direction; // Set the bullet's rotation to face the aim direction
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * BulletSpeed;
        }
    }
    
    public override void PostAction() { return; }
}