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
    public string item_nine;
    public string item_ten;
    public string item_eleven;
    public string item_twelve;
    public string item_thirteen;
    public string item_fourteen;
    public string item_fifteen;
    public string item_sixteen;
    public string item_seventeen;

    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();

    }


    public enum Items
    {
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


    public void IncreaseItemCount(Items items)
    {
        if (itemCounters.ContainsKey(items))
        {
            itemCounters[items]++;
        }
        else
        {
            itemCounters[items] = 1;
        }

        if (playerUI.one.style.display != DisplayStyle.Flex || items.ToString() == item_one)
        {
            item_one = items.ToString();
            Debug.Log(item_one);
            playerUI.one.style.display = DisplayStyle.Flex;
            playerUI.oneL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.one.style.backgroundImage = ItemS.texture;
            playerUI.one.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "1"));
            playerUI.one.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "1"));
        }
        else if (playerUI.two.style.display != DisplayStyle.Flex || items.ToString() == item_two)
        {
            item_two = items.ToString();
            playerUI.two.style.display = DisplayStyle.Flex;
            playerUI.twoL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.two.style.backgroundImage = ItemS.texture;
            playerUI.two.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "2"));
            playerUI.two.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "2"));
        }
        else if (playerUI.three.style.display != DisplayStyle.Flex || items.ToString() == item_three)
        {
            item_three = items.ToString();
            playerUI.three.style.display = DisplayStyle.Flex;
            playerUI.threeL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.three.style.backgroundImage = ItemS.texture;
            playerUI.three.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "3"));
            playerUI.three.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "3"));
        }
        else if (playerUI.four.style.display != DisplayStyle.Flex || items.ToString() == item_four)
        {
            item_four = items.ToString();
            playerUI.four.style.display = DisplayStyle.Flex;
            playerUI.fourL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.four.style.backgroundImage = ItemS.texture;
            playerUI.four.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "4"));
            playerUI.four.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "4"));
        }
        else if (playerUI.five.style.display != DisplayStyle.Flex || items.ToString() == item_five)
        {
            item_five = items.ToString();
            playerUI.five.style.display = DisplayStyle.Flex;
            playerUI.fiveL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.five.style.backgroundImage = ItemS.texture;
            playerUI.five.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "5"));
            playerUI.five.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "5"));
        }
        else if (playerUI.six.style.display != DisplayStyle.Flex || items.ToString() == item_six)
        {
            item_six = items.ToString();
            playerUI.six.style.display = DisplayStyle.Flex;
            playerUI.sixL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.six.style.backgroundImage = ItemS.texture;
            playerUI.six.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "6"));
            playerUI.six.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "6"));
        }
        else if (playerUI.seven.style.display != DisplayStyle.Flex || items.ToString() == item_seven)
        {
            item_seven = items.ToString();
            playerUI.seven.style.display = DisplayStyle.Flex;
            playerUI.sevenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.seven.style.backgroundImage = ItemS.texture;
            playerUI.seven.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "7"));
            playerUI.seven.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "7"));
        }
        else if (playerUI.eight.style.display != DisplayStyle.Flex || items.ToString() == item_eight)
        {
            item_eight = items.ToString();
            playerUI.eight.style.display = DisplayStyle.Flex;
            playerUI.eightL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.eight.style.backgroundImage = ItemS.texture;
            playerUI.eight.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "8"));
            playerUI.eight.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "8"));
        }
        else if (playerUI.nine.style.display != DisplayStyle.Flex || items.ToString() == item_nine)
        {
            item_nine = items.ToString();
            playerUI.nine.style.display = DisplayStyle.Flex;
            playerUI.nineL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.nine.style.backgroundImage = ItemS.texture;
            playerUI.nine.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "9"));
            playerUI.nine.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "9"));
        }
        else if (playerUI.ten.style.display != DisplayStyle.Flex || items.ToString() == item_ten)
        {
            item_ten = items.ToString();
            playerUI.ten.style.display = DisplayStyle.Flex;
            playerUI.tenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.ten.style.backgroundImage = ItemS.texture;
            playerUI.ten.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "10"));
            playerUI.ten.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "10"));
        }
        else if (playerUI.eleven.style.display != DisplayStyle.Flex || items.ToString() == item_eleven)
        {
            item_eleven = items.ToString();
            playerUI.eleven.style.display = DisplayStyle.Flex;
            playerUI.elevenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.eleven.style.backgroundImage = ItemS.texture;
            playerUI.eleven.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "11"));
            playerUI.eleven.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "11"));
        }
        else if (playerUI.twelve.style.display != DisplayStyle.Flex || items.ToString() == item_twelve)
        {
            item_twelve = items.ToString();
            playerUI.twelve.style.display = DisplayStyle.Flex;
            playerUI.twelveL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.twelve.style.backgroundImage = ItemS.texture;
            playerUI.twelve.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "12"));
            playerUI.twelve.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "12"));
        }
        else if (playerUI.thirteen.style.display != DisplayStyle.Flex || items.ToString() == item_thirteen)
        {
            item_thirteen = items.ToString();
            playerUI.thirteen.style.display = DisplayStyle.Flex;
            playerUI.thirteenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.thirteen.style.backgroundImage = ItemS.texture;
            playerUI.thirteen.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "13"));
            playerUI.thirteen.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "13"));
        }
        else if (playerUI.fourteen.style.display != DisplayStyle.Flex || items.ToString() == item_fourteen)
        {
            item_fourteen = items.ToString();
            playerUI.fourteen.style.display = DisplayStyle.Flex;
            playerUI.fourteenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.fourteen.style.backgroundImage = ItemS.texture;
            playerUI.fourteen.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "14"));
            playerUI.fourteen.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "14"));
        }
        else if (playerUI.fifteen.style.display != DisplayStyle.Flex || items.ToString() == item_fifteen)
        {
            item_fifteen = items.ToString();
            playerUI.fifteen.style.display = DisplayStyle.Flex;
            playerUI.fifteenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.fifteen.style.backgroundImage = ItemS.texture;
            playerUI.fifteen.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "15"));
            playerUI.fifteen.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "15"));
        }
        else if (playerUI.sixteen.style.display != DisplayStyle.Flex || items.ToString() == item_sixteen)
        {
            item_sixteen = items.ToString();
            playerUI.sixteen.style.display = DisplayStyle.Flex;
            playerUI.sixteenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.sixteen.style.backgroundImage = ItemS.texture;
            playerUI.sixteen.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "16"));
            playerUI.sixteen.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "16"));
        }
        else if (playerUI.seventeen.style.display != DisplayStyle.Flex || items.ToString() == item_seventeen)
        {
            item_seventeen = items.ToString();
            playerUI.seventeen.style.display = DisplayStyle.Flex;
            playerUI.seventeenL.text = itemCounters[items].ToString();
            ItemS = Resources.Load<Sprite>(items.ToString());
            playerUI.seventeen.style.backgroundImage = ItemS.texture;
            playerUI.seventeen.RegisterCallback<MouseEnterEvent>(evt => playerUI.OnLabelHoverEnter(evt, "17"));
            playerUI.seventeen.RegisterCallback<MouseLeaveEvent>(evt => playerUI.OnLabelHoverLeave(evt, "17"));
        }
        else { }



        Debug.Log(items.ToString() + " count: " + itemCounters[items]);
    }
}
