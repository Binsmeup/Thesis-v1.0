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
        IronHelmet,
        IronChestplate,
        IronLeggings,
        RingOfStrength,
        RingOfSpeed,
        RingOfHealth,
        DeathGem,
        PrecisionGem,
        SonicGem,
        FrenzyGem,
        TitanGem,
        VitalityNecklace,
        MightyNecklace,
        SwiftyNecklace,
        NimbleNecklace,
        DeadeyeNecklace,
        LethalNecklace,
        RingOfPrecision,
        RingOfFatality,
        RingOfHaste,
        LeatherHood,
        LeatherArmor,
        LeatherPants,
        SteelHelmet,
        SteelChestplate,
        SteelLeggings,
        TitaniumHelmet,
        TitaniumChestplate,
        TitaniumLeggings,
        JuggernautHelmet,
        JuggernautChestplate,
        JuggernautLeggings,
        DarksteelHelmet,
        DarksteelChestplate,
        DarksteelLeggings,
        Axe,
        Bat,
        Dagger,
        RareSword,
        RareSpear,
        RareAxe,
        RareBat,
        RareDagger,
        LegendSword,
        LegendSpear,
        LegendAxe,
        LegendBat,
        LegendDagger
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
