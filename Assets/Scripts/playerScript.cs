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
    public float baseDamage;
    public float damageMulti;
    public float critChance;
    public float critDamage;
    public float knockbackForce;
    float cooldown;
    public float moveSpeed;


    public string weaponType;
    public GameObject currentWeapon;
    public GameObject currentHelm;
    public GameObject currentChest;
    public GameObject currentLeg;
    public GameObject Weapon;

    Vector2 movementInput;
    Rigidbody2D rb;

    bool isMoving = false;
    private bool canRotateWeapon = true;

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

    if (cooldown <= 0f){
        if (Input.GetMouseButtonDown(0)){
            switch (weaponType){
                case "Spear":
                    anim.SetTrigger("AttackSpear");
                    attackLimit();
                    break;

                case "Sword":
                    anim.SetTrigger("AttackSword");
                    attackLimit();
                    break;

                default:
                    break;
            }
        }
    }
        else{
        cooldown -= Time.deltaTime;

            if (cooldown <= 0f){
                canRotateWeapon = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null && other.tag == "Enemy") {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;
            damagable.OnHit(baseDamage, damageMulti, critChance, critDamage, knockback);
            Debug.Log("Enemy hit");
        }
    }

    private void attackLimit(){
        cooldown = attackSpeed;
        canRotateWeapon = false;
    }


    private void FixedUpdate(){
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpearAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack")){
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

    public void getItem(ItemManagement.Items item){
    for (int i = 0; i < itemSOLibrary.itemList.Length; i++){
        if (itemSOLibrary.itemList[i].itemName == item.ToString()){
            if (itemSOLibrary.itemList[i].Type == "Weapon" || itemSOLibrary.itemList[i].Type == "Chestplate" ||
                itemSOLibrary.itemList[i].Type == "Helmet" || itemSOLibrary.itemList[i].Type == "Leggings"){
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
}

