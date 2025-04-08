using UnityEngine;

public class Items : MonoBehaviour
{
    public enum ItemType
    {
        Heal_20,
        Spd_10,
        Atk_20
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Atk_20: return 30;
            case ItemType.Spd_10: return 10;
            case ItemType.Heal_20: return 50;
        }
    }

}
