using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour{
    [SerializeField] public ItemManagement.Items item;
    [SerializeField] public bool isEquippable;

    private bool isPlayerInRange = false;
    private bool isMouseOverPickUp = false;
    private BoxCollider2D mouseCollider;

    private void Start(){
        mouseCollider = GetComponent<BoxCollider2D>();
    }

    private void Update(){
        if (isPlayerInRange && isMouseOverPickUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUpItem();
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isMouseOverPickUp = mouseCollider.OverlapPoint((Vector2)ray.origin);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            isPlayerInRange = false;
        }
    }

    private void PickUpItem(){
        playerScript player = FindObjectOfType<playerScript>();
        HealthManager playerhp = FindObjectOfType<HealthManager>();

        if (player != null){
            itemPickUp(player, playerhp);
            if (!isEquippable){
                IncreaseCounter(player);
            }
            Destroy(gameObject);
        }
    }

    private void IncreaseCounter(playerScript player){
        ItemManagement itemManagement = player.GetComponent<ItemManagement>();

        if (itemManagement != null){
            itemManagement.IncreaseItemCount(item);
        }
    }

    private void itemPickUp(playerScript player, HealthManager playerhp){
        ItemManagement itemManagement = player.GetComponent<ItemManagement>();

        if (itemManagement != null){
            ArmorHealth armorHealth = GetComponent<ArmorHealth>();
            if (armorHealth != null){
                player.SetArmorHP(armorHealth.armorHealth);
            }
            player.getItem(item);
        }
    }
}
