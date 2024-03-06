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

    public float knockbackForce;

    float cooldown;

    public float moveSpeed;

    public float collisionOffset;

    public GameObject Weapon;

    Vector2 movementInput;
    Rigidbody2D rb;

    bool isMoving = false;
    private bool canRotateWeapon = true;
  
    List<RaycastHit2D> castCollisions =  new List<RaycastHit2D> {};

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
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
                anim.SetTrigger("AttackSword");
                cooldown = attackSpeed;

                canRotateWeapon = false;
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
            damagable.OnHit(baseDamage, damageMulti, knockback);
            Debug.Log("Enemy hit");
        }
    }


    private void FixedUpdate(){
    if (movementInput != Vector2.zero)
    {
        rb.AddForce(movementInput * moveSpeed * Time.deltaTime);

        IsMoving = true;
    }else{
        IsMoving = false;
    }
}


    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }

    public void baseDamageUp(float amount){
        baseDamage += amount;
        Debug.Log("Base damage increased to: " + baseDamage);
    }

    public void moveSpeedUp(float amount){
        moveSpeed += amount;
        Debug.Log("Move Speed increased to: " + moveSpeed);
    }

}

