using UnityEngine;

public class WeaponShopTrigger : MonoBehaviour
{
    private bool isPlayerNearby = false;

    public static bool IsShopOpen { get; private set; } = false;

    public WeaponType weapon;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Buying " + weapon);
            GameController.Instance.CurrentWeapon = weapon;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
