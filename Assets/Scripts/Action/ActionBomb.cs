using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class ActionBomb : ActionBase
{
    private Vector3 mousePosition;
    private TextMeshPro cross;
    private LineRenderer lineRenderer;
    private bool isAiming = false;
    private GameObject bomb;

    private Transform playerTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float ShootingRange = 5f;
    [SerializeField] private float BulletSpeed = 10f;

    private void Start()
    {
        cross = GetComponentInChildren<TextMeshPro>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        playerTransform = GameManager.Instance.player.transform;
        
        isAiming = false;
        cross.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }
    
    public override void PreAction()
    {
        isAiming = true;
        cross.gameObject.SetActive(true);
        lineRenderer.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (isAiming)
        {
            Vector3 mouse3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mouse3D.x, mouse3D.y, 0f);
            cross.transform.position = playerTransform.position + (mousePosition - playerTransform.position).normalized * ShootingRange;
            
            lineRenderer.SetPosition(0, playerTransform.position);
            lineRenderer.SetPosition(1, cross.transform.position);
            lineRenderer.useWorldSpace = true;
        }
    }

    public override void HandleAction()
    {
        Vector3 direction = (cross.transform.position - playerTransform.position).normalized;
        bomb = Instantiate(bulletPrefab, playerTransform.position, Quaternion.identity);
        bomb.layer = LayerMask.NameToLayer("Default");
        bomb.GetComponent<Bullet>().exploding = false;
        bomb.transform.up = direction; // Set the bullet's rotation to face the aim direction
        bomb.GetComponent<Rigidbody2D>().linearVelocity = direction * BulletSpeed;
        
        isAiming = false;
        cross.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }

    public override void PostAction()
    {
        if(bomb != null)
            StartCoroutine(bomb.GetComponent<Bullet>().Explode());
    }
}