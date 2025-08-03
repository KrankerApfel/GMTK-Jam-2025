using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool outOfPlayer = false;
    
    void Start()
    {
        // Set the bullet to destroy itself after 5 seconds
        Destroy(gameObject, 4f);
        // GetComponent<Collider2D>().isTrigger = true; // Ensure the bullet can pass through the player
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
        if (!outOfPlayer)
            return;
        //if layer is player or enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || 
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(other.gameObject.name == "Foots") Destroy(other.transform.parent.gameObject);
            else Destroy(other.gameObject);
            
            Destroy(gameObject);
        }
        else
        {
            // If the bullet collides with something else, just destroy it
            Destroy(gameObject);
        }
    }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!outOfPlayer || !exploding)
    //         return;
    //     //if layer is player or enemy
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Player") || 
    //         other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         // Destroy the bullet
    //         Destroy(gameObject);
    //         Destroy(other.gameObject);
    //     }
    // }
}
