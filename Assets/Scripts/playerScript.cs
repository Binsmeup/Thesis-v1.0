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
                AudioManager.BGM.PlaySwingSound();
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
            if (movementInput != Vector2.zero) {
                if (!AudioManager.BGM.sfxSource.isPlaying)
                {
                    AudioManager.BGM.PlayWalkSoundLoop();
                }
                else
                {
                    
                }
                rb.AddForce(movementInput * moveSpeed * Time.deltaTime);
                IsMoving = true;
            }
            else
            {
                AudioManager.BGM.StopWalkSoundLoop();
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

