using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class StatsShop : MonoBehaviour
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

        public ShopItem(string name, int cost, string stat, float value)
        {
            this.name = name;
            this.cost = cost;
            this.stat = stat;
            this.value = value;
        }
    }

    private void Awake()
    {
        container = transform.Find("Container");
        if (container == null) Debug.LogError("Container not found!");

        item1 = container?.Find("Item1");
        item2 = container?.Find("Item2");
        item3 = container?.Find("Item3");
        header = container?.Find("Header");
        canvas = container?.Find("Canvas");

        if (item1 == null) Debug.LogError("Item1 not assigned!");
        if (item2 == null) Debug.LogError("Item2 not assigned!");
        if (item3 == null) Debug.LogError("Item3 not assigned!");
        if (header == null) Debug.LogError("Header not assigned!");

        item1?.gameObject.SetActive(false);
        item2?.gameObject.SetActive(false);
        item3?.gameObject.SetActive(false);
        header?.gameObject.SetActive(false);
        canvas?.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(new ShopItem("+5 Max Health", 20, "increase_base_max_hp", 5f), 0);
        CreateItemButton(new ShopItem("-0.1 Dash Cooldown", 10, "increase_base_dash_cooldown", 0.1f), 1);
        CreateItemButton(new ShopItem("+0.1 Move Speed", 10, "increase_base_move_speed", 0.1f), 2);

        header?.gameObject.SetActive(true);
        canvas?.gameObject.SetActive(true);
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
                }
            });
        }
    }
}
