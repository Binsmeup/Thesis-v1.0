using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour, IDamagable{
    public bool hasDamageCooldown = false;
    public float damageCooldownTimer;

   [SerializeField] private Slider slider;

    private float damageCooldownTimeElapsed = 0f;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private Loot loot;

    public bool _damageCooldown;
    public float maxHealth;
    public float _health;
    public float weightLevel;
    public bool _targettable;
    public TMP_Text popUptext;
    public GameObject PopupPrefab;

    public bool damageCooldown{
        get { return _damageCooldown; }
        set{
            _damageCooldown = value;
            if (_damageCooldown == true)
            {
                damageCooldownTimeElapsed = 0f;
            }
        }
    }

    public float health{
        set{
            _health = value;
            if (_health <= 0){
                if (gameObject.CompareTag("Enemy")){
                    GameObject mapGeneratorObject = GameObject.Find("MapGenerator");
                    Enemy enemy = GetComponent<Enemy>();
                        if (mapGeneratorObject != null){
                            MapGeneration mapGeneration = mapGeneratorObject.GetComponent<MapGeneration>();
                            if (mapGeneration != null){
                                mapGeneration.addKill();
                            }
                        if (enemy != null){
                            if (enemy.isBoss){
                                mapGeneration.CreatePortal();
                            }
                        
                        }
                    }
                    if (loot == null){
                        loot = GetComponent<Loot>();
                        if (loot == null){
                            loot = gameObject.AddComponent<Loot>();
                        }
                    }

                    dropItem();
                }
                if (!gameObject.CompareTag("Player")){
                Destroy(gameObject);
                }
                else if (gameObject.CompareTag("Player")){
                    playerScript script = gameObject.GetComponent<playerScript>();
                    script.isDead = true;
                    Targettable = false;
                }
            }
            if (maxHealth < _health){
                _health = maxHealth;

                   
            }
        }
        get{
            return _health;
        }
    }

    public float HelmMaxHP;
    public float HelmHP;

    public float ChestMaxHP;
    public float ChestHP;

    public float LegMaxHP;
    public float LegHP;


    public bool Targettable{
        get { return _targettable; }
        set
        {
            _targettable = value;
            physicsCollider.enabled = value;
        }
    }

    public void Start(){
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();

        slider = GetComponentInChildren<Slider>();

        if (gameObject.CompareTag("Enemy"))
        {
            slider.value = health;
            slider.maxValue = maxHealth;
        }

    }
    public void HealUp(){
        health = maxHealth;
    }

    public void OnHit(float baseDamage, float damageMulti, float critChance, float critDamage, Vector2 knockback){
        if (!damageCooldown){
            List<string> ArmorParts = new List<string>();

            if (HelmHP > 0)
                ArmorParts.Add("Helm");

            if (ChestHP > 0)
                ArmorParts.Add("Chest");

            if (LegHP > 0)
                ArmorParts.Add("Leg");

            if (ArmorParts.Count > 0){
                string randomPart = ArmorParts[Random.Range(0, ArmorParts.Count)];
                DamageArmorPart(randomPart);
                rb.AddForce((knockback), ForceMode2D.Impulse);
            }
            else{
                float critValue = Random.Range(0f, 100f);
                if (critChance >= critValue){
                    health -= (baseDamage * damageMulti * critDamage);
                    float damageValue = baseDamage * damageMulti * critDamage;
                    SpawnPopup(Vector3.zero);
                    popUptext.text = damageValue.ToString();
                    rb.AddForce((knockback), ForceMode2D.Impulse);
                    if (gameObject.CompareTag("Enemy"))
                    {
                        slider.value = health;
                        slider.maxValue = maxHealth;
                    }
                }
                else{
                    health -= (baseDamage * damageMulti);
                    SpawnPopup(Vector3.zero);
                    float damageValue = baseDamage * damageMulti;
                    popUptext.text = damageValue.ToString();
                    rb.AddForce((knockback), ForceMode2D.Impulse);
                    if (gameObject.CompareTag("Enemy"))
                    {
                        slider.value = health;
                        slider.maxValue = maxHealth;
                    }

                }
            }
        }
        if (hasDamageCooldown){
            damageCooldown = true;
        }
    }

    private void DamageArmorPart(string part){
        switch (part){
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
    private void dropItem(){
        loot.DropItem();
    }
    private void SpawnPopup(Vector3 offset)
    {
        GameObject popup = Instantiate(PopupPrefab, transform.position + offset, Quaternion.identity);
        popup.transform.SetParent(transform);
    }

    public void FixedUpdate(){
        if (damageCooldown){
            damageCooldownTimeElapsed += Time.deltaTime;

            if (damageCooldownTimeElapsed > damageCooldownTimer){
                damageCooldown = false;
            }
        }
    }
}