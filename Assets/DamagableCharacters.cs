using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableCharacters : MonoBehaviour, IDamagable
{
    public bool disableSimulation = false;
    public bool hasDamageCooldown = false;
    public float damageCooldownTimer;

    private float damageCooldownTimeElapsed = 0f;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;

    public bool _damageCooldown;
    public float _health;
    public bool _targettable;

    public bool damageCooldown
    {
        get { return _damageCooldown; }
        set
        {
            _damageCooldown = value;
            if (_damageCooldown == true)
            {
                damageCooldownTimeElapsed = 0f;
            }
        }
    }

    public float health
    {
        set
        {
            _health = value;
            if (_health <= 0)
            {
                Targettable = false;
            }
        }
        get
        {
            return _health;
        }
    }

    public bool Targettable
    {
        get { return _targettable; }
        set
        {
            _targettable = value;
            physicsCollider.enabled = value;
        }
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
    }

    public void OnHit(float baseDamage, float damageMulti, Vector2 knockback)
    {
        if (!damageCooldown){
            health -= (baseDamage * damageMulti);
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
        if (hasDamageCooldown){
            damageCooldown = true;
        }
    }

    public void OnHit(float baseDamage, float damageMulti)
    {
        if (!damageCooldown)
        {
            health -= (baseDamage * damageMulti);
        }
        if (hasDamageCooldown)
        {
            damageCooldown = true;
        }
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }

    public void FixedUpdate()
    {
        if (damageCooldown)
        {
            damageCooldownTimeElapsed += Time.deltaTime;

            if (damageCooldownTimeElapsed > damageCooldownTimer)
            {
                damageCooldown = false;
            }
        }
    }
}
