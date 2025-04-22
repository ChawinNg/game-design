using UnityEngine;

public class WeaponShopTrigger : MonoBehaviour
{
    public AlertModal alertModal; // ← Drag reference in Inspector
    public WeaponType weapon;
    public int price;
    private Player player;

    private bool isPlayerNearby = false;
    public static bool IsAlert { get; private set; } = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Buying " + weapon);

            player = FindObjectOfType<Player>();
            if(!(bool)player?.GetBuyClub() & weapon == WeaponType.Club) {
                if(player?.SpendGold(price) == true){
                    player.BuyClub();
                    GameController.Instance.CurrentWeapon = weapon;
                    alertModal.ShowAlert("Buy: " + weapon.ToString());
                    IsAlert = true;
                } else {
                    alertModal.ShowAlert("Not enough gold. Need " + price);
                    IsAlert = true;
                }
            } else if (!(bool)player?.GetBuyBow() & weapon == WeaponType.Bow){
                if(player?.SpendGold(price) == true){
                    player.BuyBow();
                    GameController.Instance.CurrentWeapon = weapon;
                    alertModal.ShowAlert("Buy: " + weapon.ToString());
                    IsAlert = true;
                } else {
                    alertModal.ShowAlert("Not enough gold. Need " + price);
                    IsAlert = true;
                }
            } else {
                alertModal.ShowAlert("Use: " + weapon.ToString());
                GameController.Instance.CurrentWeapon = weapon;
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
