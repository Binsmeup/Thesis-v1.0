using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float knockbackForce;
    public float baseDamage;
    public float damageMulti;
    public float critChance;
    public float critDamage;
    public float lifetime;

    private Vector3 direction;

    void Start(){
        StartCoroutine(ProjectileLifetime());
    }

    void Update(){
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            IDamagable damagable = other.GetComponent<IDamagable>();

            if (damagable != null){
                Vector2 knockback = direction * knockbackForce;
                damagable.OnHit(baseDamage, damageMulti, critChance, critDamage, knockback);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall")){
            Destroy(gameObject);
        }
        if (other.tag == "Debris"){
            Debris debris = other.GetComponent<Debris>();
            if (debris != null){
                debris.Destroy();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ProjectileLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
}
