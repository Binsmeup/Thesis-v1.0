using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagement : MonoBehaviour
{
    private Dictionary<Items, int> itemCounters = new Dictionary<Items, int>();

    public enum Items{
        BeginnerSword,
        Sword,
        Spear,
        Helmet,
        Chestplate,
        Leggings,
        RingOfStrength,
        RingOfSpeed,
        RingOfHealth
    }

    public void IncreaseItemCount(Items items){
        if (itemCounters.ContainsKey(items)){
            itemCounters[items]++;
        }
        else{
            itemCounters[items] = 1;
        }

        Debug.Log(items.ToString() + " count: " + itemCounters[items]);
    }
}
