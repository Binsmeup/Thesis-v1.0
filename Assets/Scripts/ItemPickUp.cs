using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] public ItemManagement.Items item;
    [SerializeField] public bool isEquippable;
    [SerializeField] public string itemName;

    private bool isPlayerInRange = false;
    private bool isMouseOverPickUp = false;
    private BoxCollider2D mouseCollider;
    private PlayerUI playerUI;

    private void Start()
    {
        mouseCollider = GetComponent<BoxCollider2D>();
        playerUI = FindObjectOfType<PlayerUI>();

    }

    private void Update()
    {
        if (isPlayerInRange && isMouseOverPickUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUpItem();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isMouseOverPickUp = mouseCollider.OverlapPoint((Vector2)ray.origin);

        if (isMouseOverPickUp)
        {
            itemName = item.ToString();
            playerUI.ShowItemDetail(itemName);
            
        }
        else
        {
            itemName = item.ToString();
            playerUI.HideItemDetail(itemName);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (isMouseOverPickUp)
            {
                playerUI.ShowFButton();
                itemName = item.ToString();
                playerUI.SetItemName(itemName);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerUI.HideFButton();
            playerUI.ClearItemName();
        }
    }

    private void PickUpItem()
    {
        playerScript player = FindObjectOfType<playerScript>();
        HealthManager playerhp = FindObjectOfType<HealthManager>();

        if (player != null)
        {
            itemPickUp(player, playerhp);
            if (!isEquippable)
            {
                IncreaseCounter(player);
            }
            Destroy(gameObject);
        }
    }

    private void IncreaseCounter(playerScript player)
    {
        ItemManagement itemManagement = player.GetComponent<ItemManagement>();

        if (itemManagement != null)
        {
            itemManagement.IncreaseItemCount(item);
        }
    }

    private void itemPickUp(playerScript player, HealthManager playerhp)
    {
        ItemManagement itemManagement = player.GetComponent<ItemManagement>();

        if (itemManagement != null)
        {
            ArmorHealth armorHealth = GetComponent<ArmorHealth>();
            if (armorHealth != null)
            {
                player.SetArmorHP(armorHealth.armorHealth);
            }
            player.getItem(item);
        }
    }
}
