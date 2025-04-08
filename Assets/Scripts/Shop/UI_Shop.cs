using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform item1;
    private Transform item2;
    private Transform item3;
    private Transform header;
    private Transform canvas;

    private void Awake()
    {
        // Check if container is assigned in the Inspector or properly found in the hierarchy
        container = transform.Find("Container");
        if (container == null)
        {
            Debug.LogError("Container not found!");
        }

        item1 = container?.Find("Item1");
        item2 = container?.Find("Item2");
        item3 = container?.Find("Item3");
        header = container?.Find("Header");
        canvas = container?.Find("Canvas");

        if (item1 == null) Debug.LogError("Item1 not assigned!");
        if (item2 == null) Debug.LogError("Item2 not assigned!");
        if (item3 == null) Debug.LogError("Item3 not assigned!");
        if (header == null) Debug.LogError("Header not assigned!");

        // Disable items initially
        item1?.gameObject.SetActive(false);
        item2?.gameObject.SetActive(false);
        item3?.gameObject.SetActive(false);
        header?.gameObject.SetActive(false);
        canvas?.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Create the items and check for errors in the creation process
        CreateItemButton("Heal 30% of Health", 50, 0, "heal", 30f);
        CreateItemButton("+10% SPD", 20, 1, "add_move_speed", 10f);
        CreateItemButton("-20% Dash CD", 30, 2, "decrease_dash_cd", 5f);
        header?.gameObject.SetActive(true);
        canvas?.gameObject.SetActive(true);
    }

    private void CreateItemButton(string itemName, int itemCost, int positionIndex, string statName, float value)
    {
        Transform itemTransform = null;

        if (positionIndex == 0)
            itemTransform = Instantiate(item1, container);
        else if (positionIndex == 1)
            itemTransform = Instantiate(item2, container);
        else if (positionIndex == 2)
            itemTransform = Instantiate(item3, container);

        itemTransform.gameObject.SetActive(true);

        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        itemRectTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        itemRectTransform.Find("Price").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        Transform descriptionBg = itemTransform.Find("Button");
        EventTrigger trigger = descriptionBg.gameObject.AddComponent<EventTrigger>();

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => {
            Debug.Log("test1");
            FindObjectOfType<Player>().UpdateStat(statName, value);
        });

        trigger.triggers.Add(entry);
    }
}
