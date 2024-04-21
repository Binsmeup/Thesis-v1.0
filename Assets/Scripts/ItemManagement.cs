using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemManagement : MonoBehaviour
{
    private Dictionary<Items, int> itemCounters = new Dictionary<Items, int>();
    private PlayerUI playerUI;
    public Sprite ItemS;
    public string item_one;
    public string item_two;
    public string item_three;
    public string item_four;
    public string item_five;
    public string item_six;
    public string item_seven;
    public string item_eight;
    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();

    }


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

        if (playerUI.one.style.display != DisplayStyle.Flex || items.ToString() == item_one)
        {
            item_one = items.ToString();
            playerUI.one.style.display = DisplayStyle.Flex;
            playerUI.oneL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.one.style.backgroundImage = ItemS.texture;
        }
        else if(playerUI.two.style.display != DisplayStyle.Flex || items.ToString() == item_two)
        {
            item_two = items.ToString();
            playerUI.two.style.display = DisplayStyle.Flex;
            playerUI.twoL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.two.style.backgroundImage = ItemS.texture;
        }
        else if (playerUI.three.style.display != DisplayStyle.Flex || items.ToString() == item_three)
        {
            item_three = items.ToString();
            playerUI.three.style.display = DisplayStyle.Flex;
            playerUI.threeL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.three.style.backgroundImage = ItemS.texture;
        }
        else if (playerUI.four.style.display != DisplayStyle.Flex || items.ToString() == item_four)
        {
            item_four = items.ToString();
            playerUI.four.style.display = DisplayStyle.Flex;
            playerUI.fourL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.four.style.backgroundImage = ItemS.texture;
        }
        else if (playerUI.five.style.display != DisplayStyle.Flex || items.ToString() == item_five)
        {
            item_five = items.ToString();
            playerUI.five.style.display = DisplayStyle.Flex;
            playerUI.fiveL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.five.style.backgroundImage = ItemS.texture;
        }
        else if (playerUI.six.style.display != DisplayStyle.Flex || items.ToString() == item_six)
        {
            item_six = items.ToString();
            playerUI.six.style.display = DisplayStyle.Flex;
            playerUI.sixL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.six.style.backgroundImage = ItemS.texture;
        }
        else if (playerUI.seven.style.display != DisplayStyle.Flex || items.ToString() == item_seven)
        {
            item_seven = items.ToString();
            playerUI.seven.style.display = DisplayStyle.Flex;
            playerUI.sevenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.seven.style.backgroundImage = ItemS.texture;
        }
        else if (playerUI.eight.style.display != DisplayStyle.Flex || items.ToString() == item_eight)
        {
            item_eight = items.ToString();
            playerUI.eight.style.display = DisplayStyle.Flex;
            playerUI.eightL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.eight.style.backgroundImage = ItemS.texture;
        }else

            Debug.Log(items.ToString() + " count: " + itemCounters[items]);
    }
}
