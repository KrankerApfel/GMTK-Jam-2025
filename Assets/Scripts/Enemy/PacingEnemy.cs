using System.Linq;
using UnityEngine;

public class PacingEnemy : Enemy
{
    Transform[] pacingPoints;
    int currentPointIndex = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        pacingPoints = transform.parent.GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - pacingPoints[currentPointIndex].position).magnitude < 0.5f)
        {
            currentPointIndex = (currentPointIndex + 1) % pacingPoints.Length;
        }

        if (! chasing)
        {
            Vector2 direction = (pacingPoints[currentPointIndex].position - transform.position).normalized;
            HandleMovement(direction.x);
        }
    }
}
