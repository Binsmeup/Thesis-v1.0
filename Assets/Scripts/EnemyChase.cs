using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public string playerTag = "Player";
    [SerializeField] public float chaseRange = 10f;
    [SerializeField] public float moveForce = 5f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float minDistanceThreshold = 0.01f;

    private Transform player;
    private Rigidbody2D rb;
    private Transform visualTransform;
    private Vector3 lastKnownPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        GameObject visualObject = new GameObject("VisualObject");
        visualTransform = visualObject.transform;
        visualTransform.parent = transform;
        visualTransform.localPosition = Vector3.zero;
    }

    private void Update(){
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange && !IsObstacleBetween()){
            lastKnownPosition = player.position;

            Vector3 direction = (player.position - transform.position).normalized;

            if (distanceToPlayer > minDistanceThreshold)
            {
                rb.AddForce(direction * moveForce * Time.deltaTime);
            }

            RotateTowardsPlayer(direction);
        }
        else if (lastKnownPosition != Vector3.zero)
        {
            Vector3 directionToLastKnown = (lastKnownPosition - transform.position).normalized;

            if (Vector3.Distance(transform.position, lastKnownPosition) > minDistanceThreshold)
            {
                rb.AddForce(directionToLastKnown * moveForce * Time.deltaTime);
            }

            RotateTowardsPlayer(directionToLastKnown);
        }
    }

    private bool IsObstacleBetween(){
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("Wall"));

        return hit.collider != null;
    }

    private void RotateTowardsPlayer(Vector3 direction){
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        visualTransform.rotation = Quaternion.RotateTowards(visualTransform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
