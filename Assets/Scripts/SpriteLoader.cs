using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    public Sprite[] sprites;

    void Start()
    {
        // Initialize the array with the correct size
        sprites = new Sprite[50]; // Assuming you have 50 items

        // Load sprites for each item
        for (int i = 0; i < 50; i++)
        {
            string itemName = GetItemName(i);
            sprites[i] = Resources.Load<Sprite>(itemName);
        }
    }

    // Helper method to get the item name based on index
    string GetItemName(int index)
    {
        switch (index)
        {
            case 0: return "BeginnerSword";
            case 1: return "Sword";
            case 2: return "Spear";
            // Add cases for other items...
            default: return ""; // Return empty string if index is out of range
        }
    }
}