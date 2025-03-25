using UnityEngine;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform item1;
    private Transform item2;
    private Transform item3;
    private Transform header;

    private void Awake()
    {
        container = transform.Find("Container");
        item1 = container.Find("Item1");
        item2 = container.Find("Item2");
        item3 = container.Find("Item3");
        header = container.Find("Header");
        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
        item3.gameObject.SetActive(false);
        header.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton("test", 3, 0);
        CreateItemButton("test2", 4, 1);
        CreateItemButton("test3", 4, 2);
        header.gameObject.SetActive(true);
    }

    private void CreateItemButton(string itemName, int itemCost, int positionIndex)
    {
        if(positionIndex == 0){
            Transform itemTransform = Instantiate(item1, container);
            itemTransform.gameObject.SetActive(true);
            RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
            itemRectTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        } else if (positionIndex == 1) {
            Transform itemTransform = Instantiate(item2, container);
            itemTransform.gameObject.SetActive(true);
            RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
            itemRectTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        } else if (positionIndex == 2) {
            Transform itemTransform = Instantiate(item3, container);
            itemTransform.gameObject.SetActive(true);
            RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
            itemRectTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        } 
    } 
}
