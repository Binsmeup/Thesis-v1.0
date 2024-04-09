using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour{
    public string playerTag = "Player";
    public float chaseRange = 10f;
    public float moveForce = 5f;
    public float rotationSpeed = 5f;
    public float minDistanceThreshold = 0.01f;
    public bool canMove = true;

    public bool canCharge;
    public float dashForce;
    public float chargeRange;
    public float chargeCooldown;
    public float lockDuration;
    public float reactionTime;

    public float endLag;

    public Transform player;
    private Rigidbody2D rb;
    private Transform visualTransform;
    private Vector3 lastKnownPosition;
    private bool isCharging = false;

    public bool isChargeCooldown = false;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        GameObject visualObject = new GameObject("VisualObject");
        visualTransform = visualObject.transform;
        visualTransform.parent = transform;
        visualTransform.localPosition = Vector3.zero;
    }

    private void Update(){
        if (isCharging || !canMove)
            return;
        if (inChargeRange() && canCharge && !isChargeCooldown){
        StartCharge();
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange && !IsObstacleBetween()){
            EnemyShoot enemyShoot = GetComponent<EnemyShoot>();
            if (enemyShoot != null){
                enemyShoot.shootReady = true;
            }
            lastKnownPosition = player.position;

            Vector3 direction = (player.position - transform.position).normalized;

            if (distanceToPlayer > minDistanceThreshold){
                rb.AddForce(direction * moveForce * Time.deltaTime);
            }

            RotateTowardsPlayer(direction);
        }
        else if (lastKnownPosition != Vector3.zero){
            EnemyShoot enemyShoot = GetComponent<EnemyShoot>();
            if (enemyShoot != null){
                enemyShoot.shootReady = false;
            }
            Vector3 directionToLastKnown = (lastKnownPosition - transform.position).normalized;

            if (Vector3.Distance(transform.position, lastKnownPosition) > minDistanceThreshold){
                rb.AddForce(directionToLastKnown * moveForce * Time.deltaTime);
            }

            RotateTowardsPlayer(directionToLastKnown);
        }
    }

    public bool IsObstacleBetween(){
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("Wall"));

        return hit.collider != null;
    }

    private void RotateTowardsPlayer(Vector3 direction){
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        visualTransform.rotation = Quaternion.RotateTowards(visualTransform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    bool inChargeRange(){
        return Vector3.Distance(transform.position, player.position) < chaseRange && !IsObstacleBetween();
    }

    public void StartCharge(){
        if (canCharge){
            if (!isCharging && Vector3.Distance(transform.position, player.position) < chargeRange){
                StartCoroutine(ChargeCoroutine());
            }
        }
    }

    private IEnumerator ChargeCoroutine(){
        Enemy enemy = GetComponent<Enemy>();
        isCharging = true;

        yield return new WaitForSeconds(lockDuration);
        Vector3 dashDirection = (player.position - transform.position).normalized;
        yield return new WaitForSeconds(reactionTime);
        enemy.damageMulti = 3;
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(endLag);
        isCharging = false;
        enemy.damageMulti = 1;
        isChargeCooldown = true;
        yield return new WaitForSeconds(chargeCooldown);
        isChargeCooldown = false;
        if (enemy.isBoss){
            BossSkills skills = GetComponent<BossSkills>();
            canCharge = false;
            StartCoroutine(skills.Cooldown(skills.skillCooldown));
        }
    }
}
