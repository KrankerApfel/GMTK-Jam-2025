using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public int speed = 2;
    protected Rigidbody2D rigidBody;
    protected bool chasing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.name == "Player")
        {
            // send a raycast to player, ignore the enemy itself
            Vector2 castOrigin = transform.position;
            Vector2 castDirection = (other.transform.position - transform.position).normalized;
            Vector2 castDistance = other.transform.position - transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(castOrigin, castDirection, castDistance.magnitude);

            bool hitPlayer = true;
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.name != "Player" 
                    && hit.collider.gameObject.name != "Foots"
                    && hit.collider.gameObject.name != "Enemy")
                {
                    hitPlayer = false;  
                    chasing = false;
                    break;
                }
            }
            
            if (hitPlayer)
            {
                chasing = true; 
                Vector2 direction = (other.transform.position - transform.position).normalized;
                HandleMovement(direction.x);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.name == "Player")
        {
            chasing = false;
            HandleMovement(0f); // stop moving when player exits trigger
        }
    }

    protected void HandleMovement(float dir)
    {
        Vector2 velocity = rigidBody.linearVelocity;
        velocity.x = speed * dir;
        rigidBody.linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(other.transform.gameObject);
        }
    }
}
