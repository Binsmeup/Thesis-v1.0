using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour{
    public GameObject projectile;
    public bool canShoot;
    public bool canSnipe;
    public float shootCooldown;
    public float bulletSpeed;
    public float lifeTime;
    public float lockDuration;
    public float reactionTime;
    public float fireCooldown;
    private float shootTimer = 0f;
    public bool shootReady = false;
    private EnemyChase enemyChase;
    private Enemy enemy;
    private BossSkills skills;

    private void Start(){
        enemyChase = GetComponent<EnemyChase>();
        enemy = GetComponent<Enemy>();
        skills = GetComponent<BossSkills>();
    }

    private void Update(){
        if (canShoot && shootReady && IsPlayerInRange()){
            if (shootTimer <= 0f){
                StartCoroutine(Shoot());
                shootTimer = shootCooldown;
            }
            else{
                shootTimer -= Time.deltaTime;
            }
        }
    }

    private IEnumerator Shoot(){
        if (enemy.isBoss){
            int bullets;
            int shots;
            if (skills.shotgunActive){
                enemyChase.canMove = false;
                if (skills.rageActive){
                    bullets = 15;
                }else{
                    bullets = 10;
                }
                shots = 1;
                yield return new WaitForSeconds(lockDuration);
                Vector3 directionToPlayer = (enemyChase.player.position - transform.position).normalized;
                StartCoroutine(Fire(directionToPlayer, bullets, shots, 60f));
            }
            if (skills.sprayActive){
                enemyChase.canMove = false;
                if (skills.rageActive){
                    bullets = 30;
                    shots = 6;
                }else{
                    bullets = 20;
                    shots = 4;
                }
                yield return new WaitForSeconds(lockDuration);
                Vector3 directionToPlayer = (enemyChase.player.position - transform.position).normalized;
                StartCoroutine(Fire(directionToPlayer, bullets, shots, 360f));
            }
        }
        else
        {
            if (canSnipe){
                yield return new WaitForSeconds(lockDuration);
                Vector3 directionToPlayer = (enemyChase.player.position - transform.position).normalized;
                yield return new WaitForSeconds(reactionTime);
                StartCoroutine(Fire(directionToPlayer, 1, 1, 0f));
            }
            else{
                Vector3 directionToPlayer = (enemyChase.player.position - transform.position).normalized;
                StartCoroutine(Fire(directionToPlayer, 1, 1, 0f));
            }
        }
    }

    private IEnumerator Fire(Vector3 directionToPlayer, int projectileCount, int fireCount, float spread){
        if (enemy.isBoss){
            if (skills.shotgunActive){
                skills.shotgunActive = false;
                canShoot = false;
            }
            if (skills.sprayActive){
                skills.sprayActive = false;
                canShoot = false;
            }
        }
        for (int a = 0 ; a < fireCount; a++){
            for (int i = 0; i < projectileCount; i++){
                Vector3 spreadDirection = Quaternion.Euler(0, 0, Random.Range(-spread, spread)) * directionToPlayer;

                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                Projectile projectileScript = newProjectile.GetComponent<Projectile>();

                if (projectileScript != null){
                    projectileScript.SetDirection(spreadDirection);
                    projectileScript.speed = bulletSpeed;
                    projectileScript.knockbackForce = enemy.knockbackForce;
                    projectileScript.baseDamage = enemy.baseDamage;
                    projectileScript.damageMulti = enemy.damageMulti;
                    projectileScript.critChance = enemy.critChance;
                    projectileScript.critDamage = enemy.critDamage;
                    projectileScript.lifetime = lifeTime;
                }
            }
            yield return new WaitForSeconds(fireCooldown);
        }
        if (enemy.isBoss){
            yield return new WaitForSeconds(enemyChase.endLag);
            enemyChase.canMove = true;
            StartCoroutine(skills.Cooldown(skills.skillCooldown));
        }
    }

    private bool IsPlayerInRange(){
        return Vector3.Distance(transform.position, enemyChase.player.position) < enemyChase.chaseRange;
    }
}
