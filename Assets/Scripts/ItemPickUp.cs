using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] public ItemManagement.Items item; // Use the same enum type

    private bool isPlayerInRange = false;

    public enum ItemName
    {
        RingOfStrength,
        RingOfSpeed,
        RingOfHealth
        // Add more item types as needed
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PickUpItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void PickUpItem()
    {
        playerScript player = FindObjectOfType<playerScript>(); // Assuming there's only one player or modify accordingly

        if (player != null)
        {
            ApplyEffect(player);
            IncreaseCounter(player);
            Destroy(gameObject); // Remove the item from the scene after pickup
        }
    }

    private void ApplyEffect(playerScript player)
    {
        switch (item)
        {
            case ItemManagement.Items.RingOfStrength:
                player.baseDamageUp(5);
                Debug.Log("Ring of Strength picked up!");
                break;
            
            case ItemManagement.Items.RingOfSpeed:
                player.moveSpeedUp(50);
                Debug.Log("Ring of Strength picked up!");
                break;

            case ItemManagement.Items.RingOfHealth:
                Debug.Log("Ring of Health picked up!");
                break;


            default:
                Debug.LogWarning("Unhandled item type: " + item);
                break;
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
}
