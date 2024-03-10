using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public string playerTag = "Player";
    [SerializeField] public float chaseRange = 10f;
    [SerializeField] public float moveForce = 5f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float minDistanceThreshold = 0.01f; // Minimum distance threshold to apply force

    private Transform player;
    private Rigidbody2D rb;
    private Transform visualTransform; // Separate transform for visuals
    private Vector3 lastKnownPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        // Create an empty GameObject as a child to handle the visual representation
        GameObject visualObject = new GameObject("VisualObject");
        visualTransform = visualObject.transform;
        visualTransform.parent = transform;
        visualTransform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        // Check if the player is within the chase range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange && !IsObstacleBetween())
        {
            // Update last known position when player is in range and not obstructed
            lastKnownPosition = player.position;

            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Move towards the player using force if not too close
            if (distanceToPlayer > minDistanceThreshold)
            {
                rb.AddForce(direction * moveForce * Time.deltaTime);
            }

            // Rotate towards the player
            RotateTowardsPlayer(direction);
        }
        else
        {
            // Calculate the direction to the last known position
            Vector3 directionToLastKnown = (lastKnownPosition - transform.position).normalized;

            // Move towards the last known position using force if not too close
            if (Vector3.Distance(transform.position, lastKnownPosition) > minDistanceThreshold)
            {
                rb.AddForce(directionToLastKnown * moveForce * Time.deltaTime);
            }

            // Rotate towards the last known position
            RotateTowardsPlayer(directionToLastKnown);
        }
    }

    private bool IsObstacleBetween()
    {
        // Cast a ray from the enemy to the player and check for obstacles
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("Wall"));

        // If the ray hits an obstacle, return true
        return hit.collider != null;
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        // Rotate the visualTransform towards the player only when actively chasing
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        visualTransform.rotation = Quaternion.RotateTowards(visualTransform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
