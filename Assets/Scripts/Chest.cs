using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour{
    private bool isPlayerInRange = false;
    private bool isMouseOverPickUp = false;
    private BoxCollider2D mouseCollider;
    private Loot loot;

    private PlayerUI playerUI;
    private void Start(){
        mouseCollider = GetComponent<BoxCollider2D>();
        loot = gameObject.GetComponent<Loot>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    private void Update(){
        if (isPlayerInRange && isMouseOverPickUp && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isMouseOverPickUp = mouseCollider.OverlapPoint((Vector2)ray.origin);

        if (isMouseOverPickUp && isPlayerInRange)
        {
                playerUI.ShowChestButton();

        }
        else
        {
            playerUI.HideChestButton();
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            isPlayerInRange = false;
            playerUI.HideChestButton();
        }
    }

    private void OpenChest(){
        playerScript player = FindObjectOfType<playerScript>();
        HealthManager playerhp = FindObjectOfType<HealthManager>();

        if (player != null){
            loot.DropItem();
            Destroy(gameObject);
        }
    }
}
