using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class ActionGrapple: ActionBase
{
    private Vector3 mousePosition;
    private TextMeshPro cross;
    private LineRenderer lineRenderer;
    private bool isAiming = false;
    private bool isGrappling = false;

    private Transform playerTransform;
    private PlayerPhysics playerPhysics;
    [SerializeField] private float ShootingRange = 5f;
    [SerializeField] private float speed = 30f;

    private void Start()
    {
        cross = GetComponentInChildren<TextMeshPro>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        playerTransform = GameManager.Instance.player.transform;
        playerPhysics = GameManager.Instance.player.GetComponent<PlayerPhysics>();
        
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

        if (isGrappling)
        {
            lineRenderer.SetPosition(0, playerTransform.position);
            lineRenderer.SetPosition(1, cross.transform.position);
            lineRenderer.useWorldSpace = true;
        }
    }

    public override void HandleAction()
    {
        isGrappling = true;
        isAiming = false;
        StartCoroutine(GrappleToTarget(cross.transform.position));
    }
    
    private IEnumerator GrappleToTarget(Vector3 targetPosition)
    {
        float duration = 0.1f;
        float elapsedTime = 0f;
        Rigidbody2D rigidBody = playerTransform.GetComponent<Rigidbody2D>();
        
        playerPhysics.DisableMovement(true); // Disable player movement during grapple
        float originalGravityScale = playerPhysics.GetComponent<Rigidbody2D>().gravityScale;
        playerPhysics.GetComponent<Rigidbody2D>().gravityScale = 0f;
        while ((playerTransform.position- targetPosition).sqrMagnitude > 0.01f && elapsedTime < duration)
        {
            rigidBody.linearVelocity = (targetPosition - playerTransform.position).normalized * speed;
            
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        playerPhysics.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale; // Reset gravity scale
        playerPhysics.DisableMovement(false); // Re-enable player movement after grapple
        // rigidBody.linearVelocity = Vector2.zero; // Stop the player after grappling
        
        isGrappling = false;
        lineRenderer.gameObject.SetActive(false);
        cross.gameObject.SetActive(false);
    }
    
    public override void PostAction() { return; }
}