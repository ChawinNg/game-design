using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemClickHandler : MonoBehaviour, IPointerClickHandler
{
    public string statName;
    public float value;

    public void OnPointerClick(PointerEventData eventData)
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Debug.Log("test");
            // player.UpdateStat(statName, value);
        }
    }
}
