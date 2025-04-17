using UnityEngine;

public class WeaponShopTrigger : MonoBehaviour
{
    public AlertModal alertModal; // ← Drag reference in Inspector
    public WeaponType weapon;

    private bool isPlayerNearby = false;
    public static bool IsAlert { get; private set; } = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Buying " + weapon);

            GameController.Instance.CurrentWeapon = weapon;

            // Show modal
            if (alertModal != null)
            {
                alertModal.ShowAlert("Use: " + weapon.ToString());
                IsAlert = true;
            }
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

            if (alertModal != null)
            {
                alertModal.HideAlert();
                IsAlert = false;
            }
        }
    }
}
