using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI; // Assign the UI_Shop GameObject in the Inspector
    private bool isPlayerNearby = false;

    public static bool IsShopOpen { get; private set; } = false;

    private void Start()
    {
        shopUI.SetActive(false); // Ensure shop UI is hidden initially
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            bool isActive = !shopUI.activeSelf;
            shopUI.SetActive(isActive); // Toggle shop UI
            IsShopOpen = isActive; // Toggle shop UI
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            shopUI.SetActive(false);
            IsShopOpen = false;
        }
    }
}
