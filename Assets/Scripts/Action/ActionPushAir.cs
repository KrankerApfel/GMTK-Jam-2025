using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class ActionPushAir : ActionBase
{
    private Vector3 mousePosition;
    private TextMeshPro cross;
    private LineRenderer lineRenderer;
    private bool isAiming = false;

    private Transform playerTransform;
    [SerializeField] private float ShootingRange = 5f;
    [SerializeField] private float speed;

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
        isAiming = false;
        cross.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
        
        StartCoroutine(PushAir());
    }

    private IEnumerator PushAir()
    {
        float duration = 0.1f;
        float elapsedTime = 0f;
        
        
        Vector3 direction = (cross.transform.position - playerTransform.position).normalized;
        PlayerPhysics playerPhysics = playerTransform.GetComponent<PlayerPhysics>();
        Rigidbody2D rigidBody = playerTransform.GetComponent<Rigidbody2D>();
        
        playerPhysics.DisableMovement(true);
        rigidBody.linearVelocity = - direction * speed;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        // rigidBody.linearVelocity = Vector2.zero;
        playerPhysics.DisableMovement(false);
    }
    
    
    public override void PostAction() { return; }
}