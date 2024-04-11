using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int goldMin;
    public int goldMax;
    public float itemDropChance;
    public float legendaryChance;
    public float rareChance;

    public List<ItemList> legendaryItems;
    public List<ItemList> rareItems;
    public List<ItemList> commonItems;

    public void DropItem(){
        float dropChance = Random.Range(0f, 100f);
        int goldDrop = Random.Range(goldMin, goldMax);
        GameObject player = GameObject.Find("Player");
        if (player != null){
            playerScript script = player.GetComponent<playerScript>();
            if (script != null){
                script.getCoins(goldDrop);
            }
        }

        if (dropChance <= itemDropChance){
            float legendaryItemPool = Random.Range(0f, 100f);
            float rareItemPool = Random.Range(0f, 100f);

            if (legendaryItemPool <= legendaryChance){
                DropRandomItem(legendaryItems);
            }
            else if (rareItemPool <= rareChance){
                DropRandomItem(rareItems);
            }
            else{
                DropRandomItem(commonItems);
            }
        }
    }

    private void DropRandomItem(List<ItemList> itemPool){
        if (itemPool.Count > 0){
            int randomIndex = Random.Range(0, itemPool.Count);
            ItemList selectedItem = itemPool[randomIndex];

            Instantiate(selectedItem.itemObject, transform.position, Quaternion.identity);
        }
    }
}