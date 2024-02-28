using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{
    [SerializeField] public float health;

    [SerializeField] public float knockbackForce;

    [SerializeField] public float baseDamage;

    [SerializeField] public float damageMulti;

private void OnCollisionEnter2D(Collision2D other){
    Collider2D collider = other.collider;
    IDamagable damagable = collider.GetComponent<IDamagable>();

    if(damagable != null){
        Vector2 direction = (collider.transform.position - transform.position).normalized;

        Vector2 knockback = direction * knockbackForce;

        damagable.OnHit(baseDamage, damageMulti, knockback);
    }
}
}

