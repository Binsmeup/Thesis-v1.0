using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float attackSpeed;

    [SerializeField] private float damage;

    private bool canRotateWeapon = true;

    float cooldown;

    public float moveSpeed;

    public float collisionOffset;

    public GameObject Weapon;

    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;
  
    List<RaycastHit2D> castCollisions =  new List<RaycastHit2D> {};

private void Start()
{
    rb = GetComponent<Rigidbody2D>();
    Weapon = GameObject.Find("Weapon");
}

private void Update(){
    //weapon facing to where ur mouse is
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
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("AttackSword");
            cooldown = attackSpeed;

            canRotateWeapon = false;
        }
    }
    else{
        cooldown -= Time.deltaTime;

        if (cooldown <= 0f)
        {
            canRotateWeapon = true;
        }
    }
    //checks if ur character isnt moving
    if (movementInput == Vector2.zero){
        anim.SetBool("Moving", false);
    }
}
private void OnTriggerEnter2D(Collider2D other){
    if (other.tag == "Enemy") {
        Debug.Log("Enemy hit");
    }
}


private void FixedUpdate(){
    if (movementInput != Vector2.zero) {
        int count = rb.Cast(
        movementInput,
        movementFilter,
        castCollisions,
        moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        }
    }
    
}


void OnMove(InputValue movementValue){
    movementInput = movementValue.Get<Vector2>();
    anim.SetBool("Moving", movementInput != Vector2.zero);
    }
}
