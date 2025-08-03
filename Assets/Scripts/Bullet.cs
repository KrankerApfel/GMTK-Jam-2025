using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool outOfPlayer = false;
    public bool exploding = true;
    
    void Start()
    {
        // Set the bullet to destroy itself after 5 seconds
        Destroy(gameObject, 5f);
        GetComponent<Collider2D>().isTrigger = true; // Ensure the bullet can pass through the player
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the bullet is out of the player
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            outOfPlayer = true;
            GetComponent<Collider2D>().isTrigger = false; // Disable trigger to allow collisions with enemies
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!outOfPlayer || !exploding)
            return;
        //if layer is player or enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || 
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Destroy the bullet
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!outOfPlayer || !exploding)
            return;
        //if layer is player or enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || 
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Destroy the bullet
            // Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    public IEnumerator Explode()
    {
        exploding = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        GetComponent<Collider2D>().isTrigger = true;
        
        float duration = 0.1f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 3;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            if(this != null) transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.1f);
        
        if(this != null){
            transform.localScale = targetScale;
            Destroy(gameObject); 
        }
    }
}
