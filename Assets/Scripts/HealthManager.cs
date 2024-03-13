using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamagable
{
    public bool hasDamageCooldown = false;
    public float damageCooldownTimer;

    private float damageCooldownTimeElapsed = 0f;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private Loot loot;

    public bool _damageCooldown;
    public float maxHealth;
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
                if (gameObject.CompareTag("Enemy"))
                {
                    if (loot == null)
                    {
                        loot = GetComponent<Loot>();
                        if (loot == null)
                        {
                            loot = gameObject.AddComponent<Loot>();
                        }
                    }

                    dropItem();
                }
                Destroy(gameObject);
            }
            if (maxHealth < _health)
            {
                _health = maxHealth;
            }
        }
        get
        {
            return _health;
        }
    }

    public float HelmMaxHP;
    public float HelmHP;

    public float ChestMaxHP;
    public float ChestHP;

    public float LegMaxHP;
    public float LegHP;


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
    public void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Player's Health: " + _health);
            Debug.Log("Player's Max Health: " + maxHealth);
        }
    }

    public void OnHit(float baseDamage, float damageMulti, float critChance, float critDamage, Vector2 knockback)
    {
        if (!damageCooldown)
        {
            List<string> ArmorParts = new List<string>();

            if (HelmHP > 0)
                ArmorParts.Add("Helm");

            if (ChestHP > 0)
                ArmorParts.Add("Chest");

            if (LegHP > 0)
                ArmorParts.Add("Leg");

            if (ArmorParts.Count > 0)
            {
                string randomPart = ArmorParts[Random.Range(0, ArmorParts.Count)];
                DamageArmorPart(randomPart);
                rb.AddForce(knockback, ForceMode2D.Impulse);
            }
            else
            {
                float critValue = Random.Range(0f, 100f);
                if (critChance >= critValue)
                {
                    health -= (baseDamage * damageMulti * critDamage);
                    rb.AddForce(knockback, ForceMode2D.Impulse);
                }
                else
                {
                    health -= (baseDamage * damageMulti);
                    rb.AddForce(knockback, ForceMode2D.Impulse);

                }
            }
        }
        if (hasDamageCooldown)
        {
            damageCooldown = true;
        }
    }

    public void OnHit(float baseDamage, float damageMulti, float critChance, float critDamage)
    {
        if (!damageCooldown)
        {
            List<string> partsWithHP = new List<string>();
            if (HelmHP > 0)
                partsWithHP.Add("Helm");

            if (ChestHP > 0)
                partsWithHP.Add("Chest");

            if (LegHP > 0)
                partsWithHP.Add("Leg");

            if (partsWithHP.Count > 0)
            {
                string randomPart = partsWithHP[Random.Range(0, partsWithHP.Count)];
                DamageArmorPart(randomPart);
            }
            else
            {
                float critValue = Random.Range(0f, 100f);
                if (critChance >= critValue)
                {
                    health -= (baseDamage * damageMulti * critDamage);
                }
                else
                {
                    health -= (baseDamage * damageMulti);
                }
            }
        }
        if (hasDamageCooldown)
        {
            damageCooldown = true;
        }
    }

    private void DamageArmorPart(string part)
    {
        switch (part)
        {
            case "Helm":
                HelmHP -= 1;
                break;
            case "Chest":
                ChestHP -= 1;
                break;
            case "Leg":
                LegHP -= 1;
                break;
        }
    }

    private void dropItem()
    {
        loot.DropItem();
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