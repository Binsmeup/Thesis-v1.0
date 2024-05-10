using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{
    public float knockbackForce;
    public float baseDamage;
    public float damageMulti;
    public float critChance;
    public float critDamage;

    public bool isBoss;

    private void OnCollisionEnter2D(Collision2D other){
        Collider2D collider = other.collider;

        if (collider.CompareTag("Player")){
            IDamagable damagable = collider.GetComponent<IDamagable>();

            if (damagable != null){
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                Vector2 knockback = direction * knockbackForce;
                damagable.OnHit(baseDamage, damageMulti, critChance, critDamage, knockback);
            }
        }
        else if (collider.CompareTag("Debris")){
            Debris debris = collider.GetComponent<Debris>();
            if (debris != null){
                debris.Destroy();
            }
        }
    }
}
