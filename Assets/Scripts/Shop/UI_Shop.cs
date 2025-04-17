using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform item1;
    private Transform item2;
    private Transform item3;
    private Transform header;
    private Transform canvas;

    [System.Serializable]
    public class ShopItem
    {
        public string name;
        public int cost;
        public string stat;
        public float value;
        public Sprite icon;

        public ShopItem(string name, int cost, string stat, float value, Sprite icon)
        {
            this.name = name;
            this.cost = cost;
            this.stat = stat;
            this.value = value;
            this.icon = icon;
        }
    }

    public Sprite healSprite;
    public Sprite speedSprite;
    public Sprite dashSprite;
    public Sprite maxHpSprite;
    public Sprite goldSprite;

    private List<ShopItem> allItems;

    private void Awake()
    {
        container = transform.Find("Container");
        item1 = container?.Find("Item1");
        item2 = container?.Find("Item2");
        item3 = container?.Find("Item3");
        header = container?.Find("Header");
        canvas = container?.Find("Canvas");

        item1?.gameObject.SetActive(false);
        item2?.gameObject.SetActive(false);
        item3?.gameObject.SetActive(false);
        header?.gameObject.SetActive(false);
        canvas?.gameObject.SetActive(false);

        // Initialize items with sprites
        allItems = new List<ShopItem>
        {
            new ShopItem("Heal 30% of Health", 50, "heal", 30f, healSprite),
            new ShopItem("Heal 20% of Health", 35, "heal", 20f, healSprite),
            new ShopItem("Heal 10% of Health", 20, "heal", 10f, healSprite),
            new ShopItem("+10% SPD", 20, "add_move_speed", 10f, speedSprite),
            new ShopItem("+15% SPD", 30, "add_move_speed", 15f, speedSprite),
            new ShopItem("-10% Dash CD", 20, "decrease_dash_cd", 10f, dashSprite),
            new ShopItem("-20% Dash CD", 30, "decrease_dash_cd", 20f, dashSprite),
            new ShopItem("+50 Max HP", 35, "increase_max_hp", 50f, maxHpSprite),
            new ShopItem("+100 Max HP", 65, "increase_max_hp", 100f, maxHpSprite),
            new ShopItem("Gain +15% More Gold", 50, "gold_mult", 15f, goldSprite),
            new ShopItem("Gain +5% More Gold", 10, "gold_mult", 5f, goldSprite),
        };
    }

    private void Start()
    {
        List<ShopItem> selectedItems = GetRandomItems(3);

        for (int i = 0; i < selectedItems.Count; i++)
        {
            CreateItemButton(selectedItems[i], i);
        }

        header?.gameObject.SetActive(true);
        canvas?.gameObject.SetActive(true);
    }

    private List<ShopItem> GetRandomItems(int count)
    {
        List<ShopItem> copy = new List<ShopItem>(allItems);
        List<ShopItem> selected = new List<ShopItem>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int randIndex = Random.Range(0, copy.Count);
            selected.Add(copy[randIndex]);
            copy.RemoveAt(randIndex);
        }

        return selected;
    }

    private void CreateItemButton(ShopItem shopItem, int positionIndex)
    {
        Transform itemTransform = null;

        if (positionIndex == 0) itemTransform = item1;
        else if (positionIndex == 1) itemTransform = item2;
        else if (positionIndex == 2) itemTransform = item3;

        if (itemTransform == null) return;

        itemTransform.gameObject.SetActive(true);

        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        itemRectTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(shopItem.name);
        itemRectTransform.Find("Price").GetComponent<TextMeshProUGUI>().SetText(shopItem.cost.ToString());

        Transform iconTransform = itemRectTransform.Find("ItemImg");
        if (iconTransform != null)
        {
            Image iconImage = iconTransform.GetComponent<Image>();
            if (iconImage != null && shopItem.icon != null)
            {
                iconImage.sprite = shopItem.icon;
            }
        }

        // Set up the button
        Button uiButton = itemTransform.Find("Button")?.GetComponent<Button>();
        if (uiButton != null)
        {
            uiButton.onClick.RemoveAllListeners(); // Clean old listeners
            uiButton.onClick.AddListener(() =>
            {
                if(FindObjectOfType<Player>()?.SpendGold(shopItem.cost) == true){
                    Debug.Log("Bought: " + shopItem.name);
                    FindObjectOfType<Player>()?.UpdateStat(shopItem.stat, shopItem.value);
                    uiButton.interactable = false; // Disable button after purchase
                    SafeSetActive(itemTransform, "Price", false);
                    SafeSetActive(itemTransform, "PriceIcon", false);
                    SafeSetActive(itemTransform, "PriceBg", false);
                }
            }); 
        }
    }

    private void SafeSetActive(Transform parent, string name, bool state)
    {
        Transform target = parent.Find(name);
        if (target != null) target.gameObject.SetActive(state);
    }
}
