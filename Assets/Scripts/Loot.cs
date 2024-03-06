using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public float goldMin;
    public float goldMax;
    public float itemDropChance;
    public float legendaryChance;
    public float rareChance;

    public List<ItemList> legendaryItems;
    public List<ItemList> rareItems;
    public List<ItemList> commonItems;

    public void DropItem()
    {
        float dropChance = Random.Range(0f, 1f);

        if (dropChance <= itemDropChance)
        {
            float legendaryItemPool = Random.Range(0f, 1f);
            float rareItemPool = Random.Range(0f, 1f);

            if (legendaryItemPool <= legendaryChance)
            {
                DropRandomItem(legendaryItems);
            }
            else if (rareItemPool <= rareChance)
            {
                DropRandomItem(rareItems);
            }
            else
            {
                DropRandomItem(commonItems);
            }
        }
    }

    private void DropRandomItem(List<ItemList> itemPool)
    {
        if (itemPool.Count > 0)
        {
            int randomIndex = Random.Range(0, itemPool.Count);
            ItemList selectedItem = itemPool[randomIndex];

            Instantiate(selectedItem.itemObject, transform.position, Quaternion.identity);
        }
    }
}