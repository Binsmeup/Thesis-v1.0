using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour{

    bool IsMoving {
        set{
            isMoving = value;
            anim.SetBool("Moving", isMoving);
        }
    }

    public Animator anim;
    public float attackSpeed;
    public float attackSpeedModifier;
    public float baseDamage;
    public float damageMulti;
    public float critChance;
    public float critDamage;
    public float knockbackForce;
    float cooldown;
    public float moveSpeed;

    public int coins;


    public string weaponType;
    public GameObject currentWeapon;
    public GameObject currentHelm;
    public GameObject currentChest;
    public GameObject currentLeg;
    public GameObject Weapon;

    Vector2 movementInput;
    Rigidbody2D rb;

    bool isMoving = false;

    public bool canAttack = true;
    public bool isDead = false;
    public bool canRotateWeapon = true;

    private ItemSOLibrary itemSOLibrary;

    public float tempArmorHP;
  

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        itemSOLibrary = GameObject.Find("ItemList").GetComponent<ItemSOLibrary>();
        Weapon = GameObject.Find("Weapon");
    }

    private void Update(){
        if (Weapon != null){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            if (canRotateWeapon){
                Weapon.transform.up = direction;

                if (direction.x < 0){
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else{
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        if (canAttack && !isDead){
            if (Input.GetMouseButtonDown(0)){
                    switch (weaponType){
                    case "Spear":
                        canAttack = false;
                        canRotateWeapon = false;
                        anim.SetTrigger("AttackSpear");
                        attackLimit();
                    break;

                    case "Sword":
                        canAttack = false;
                        canRotateWeapon = false;
                        anim.SetTrigger("AttackSword");
                        attackLimit();
                    break;
                    case "Axe":
                        canAttack = false;
                        canRotateWeapon = false;
                        anim.SetTrigger("AttackAxe");
                        attackLimit();
                    break;
                    case "Bat":
                        canAttack = false;
                        canRotateWeapon = false;
                        anim.SetTrigger("AttackBat");
                        attackLimit();
                    break;
                    case "Dagger":
                        canAttack = false;
                        canRotateWeapon = false;
                        anim.SetTrigger("AttackDagger");
                        attackLimit();
                    break;

                    default:
                    break;
                }
            }
        }
    }

    public void OnAttackAnimationComplete() {
        StartCoroutine(attackLimit());
    }
    private void OnTriggerEnter2D(Collider2D other){
        float trueKnockbackForce;
        IDamagable damagable = other.GetComponent<IDamagable>();
        HealthManager healthManager = other.GetComponent<HealthManager>();
        if (damagable != null && healthManager != null && other.tag == "Enemy") { 
            float weightLevel = healthManager.weightLevel;
            trueKnockbackForce = Mathf.Max(0.5f, knockbackForce - weightLevel);
            Vector2 direction = (other.transform.position - transform.position).normalized;
            Vector2 knockback = direction * trueKnockbackForce;
            damagable.OnHit(baseDamage, damageMulti, critChance, critDamage, knockback);
        }
        else if (other.tag == "Debris"){
            Debris debris = other.GetComponent<Debris>();
            if (debris != null){
                debris.Destroy();
            }
        }
    }

    private IEnumerator attackLimit(){
        cooldown = attackSpeed/attackSpeedModifier;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
        canRotateWeapon = true;
    }


    private void FixedUpdate(){
        if (!isDead && !anim.GetCurrentAnimatorStateInfo(0).IsName("SpearAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("AxeAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("BatAttack")){
            if (movementInput != Vector2.zero){
                rb.AddForce(movementInput * moveSpeed * Time.deltaTime);
                IsMoving = true;
            }
            else{
                IsMoving = false;
            }
        }
        else{
        IsMoving = false;
        }
    }


    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }

    public void getItem(ItemManagement.Items item)
    {
        for (int i = 0; i < itemSOLibrary.itemList.Length; i++)
        {
            if (itemSOLibrary.itemList[i].itemName == item.ToString())
            {
                Debug.Log("Picked up item: " + itemSOLibrary.itemList[i].itemName);
                if (itemSOLibrary.itemList[i].baseDmg > 0)
                    Debug.Log("Base Damage: " + itemSOLibrary.itemList[i].baseDmg);
                if (itemSOLibrary.itemList[i].dmgMulti > 0)
                    Debug.Log("Damage Multiplier: " + itemSOLibrary.itemList[i].dmgMulti);
                if (itemSOLibrary.itemList[i].critCHA > 0)
                    Debug.Log("Critical Chance: " + itemSOLibrary.itemList[i].critCHA);
                if (itemSOLibrary.itemList[i].critDMG > 0)
                    Debug.Log("Critical Damage: " + itemSOLibrary.itemList[i].critDMG);
                if (itemSOLibrary.itemList[i].atkSPD > 0)
                    Debug.Log("Attack Speed: " + itemSOLibrary.itemList[i].atkSPD);
                if (itemSOLibrary.itemList[i].atkSPDModifier > 0)
                    Debug.Log("Attack Speed Modifier: " + itemSOLibrary.itemList[i].atkSPDModifier);
                if (itemSOLibrary.itemList[i].maxHP > 0)
                    Debug.Log("Max HP: " + itemSOLibrary.itemList[i].maxHP);
                if (itemSOLibrary.itemList[i].HP > 0)
                    Debug.Log("HP: " + itemSOLibrary.itemList[i].HP);
                if (itemSOLibrary.itemList[i].KB > 0)
                    Debug.Log("Knockback: " + itemSOLibrary.itemList[i].KB);
                if (itemSOLibrary.itemList[i].MS > 0)
                    Debug.Log("Move Speed: " + itemSOLibrary.itemList[i].MS);
                if (itemSOLibrary.itemList[i].HelmetMaxHealth > 0)
                    Debug.Log("Helmet Max Health: " + itemSOLibrary.itemList[i].HelmetMaxHealth);
                if (itemSOLibrary.itemList[i].ChestMaxHealth > 0)
                    Debug.Log("Chestplate Max Health: " + itemSOLibrary.itemList[i].ChestMaxHealth);
                if (itemSOLibrary.itemList[i].LegMaxHealth > 0)
                    Debug.Log("Leggings Max Health: " + itemSOLibrary.itemList[i].LegMaxHealth);

                if (itemSOLibrary.itemList[i].Type == "Weapon" || itemSOLibrary.itemList[i].Type == "Chestplate" ||
                    itemSOLibrary.itemList[i].Type == "Helmet" || itemSOLibrary.itemList[i].Type == "Leggings")
                {
                    itemDrop(item);
                }

                itemSOLibrary.itemList[i].onEquip();
            }
        }
    }

    public void itemDrop(ItemManagement.Items item){
        for (int i = 0; i < itemSOLibrary.itemList.Length; i++){
            if (itemSOLibrary.itemList[i].itemName == item.ToString()){
                switch (itemSOLibrary.itemList[i].Type){
                    case "Weapon":
                        if (currentWeapon != null){
                            for (int j = 0; j < itemSOLibrary.itemList.Length; j++){
                                if (itemSOLibrary.itemList[j].itemName == currentWeapon.name){
                                    itemSOLibrary.itemList[j].onUnequip();
                                    break;
                                }
                            }
                        }
                        break;
                    case "Helmet":
                        if (currentHelm != null){
                            for (int j = 0; j < itemSOLibrary.itemList.Length; j++){
                                if (itemSOLibrary.itemList[j].itemName == currentHelm.name){
                                    itemSOLibrary.itemList[j].onUnequip();
                                    break;
                                }
                            }
                        }
                        break;
                    case "Chestplate":
                        if (currentChest != null){
                            for (int j = 0; j < itemSOLibrary.itemList.Length; j++){
                                if (itemSOLibrary.itemList[j].itemName == currentChest.name){
                                    itemSOLibrary.itemList[j].onUnequip();
                                    break;
                                }
                            }
                        }
                        break;
                    case "Leggings":
                        if (currentLeg != null){
                            for (int j = 0; j < itemSOLibrary.itemList.Length; j++){
                                if (itemSOLibrary.itemList[j].itemName == currentLeg.name){
                                    itemSOLibrary.itemList[j].onUnequip();
                                    break;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
    public void SetArmorHP(float value){
        tempArmorHP = value;
    }
    public float GetArmorHP(){
        return tempArmorHP;
    }

    public void getCoins(int coinGain){
        coins += coinGain;
    }
}

