using UnityEngine;

[CreateAssetMenu(fileName = "New Augment", menuName = "Game/Augment")]
public class AugmentData : ScriptableObject
{
    public string augmentName;
    public Sprite iconSprite;
    public float width;
    public float height;
    public string description;
    public float healthModifier;
    public float damageModifier;
    public float moveSpeedModifer;
    public float dashCooldownModifier;
    public int armorModifier;
    public float attackSpeedModifier;
    public void ApplyEffect()
    {
        Player player = FindFirstObjectByType<Player>();
        Player playerScript = player.GetComponent<Player>();

        if (playerScript != null)
        {
            if (healthModifier != 0)
            {
                AugmentStore.Instance.HealthModifier += healthModifier; 
                // playerScript.OnHealthChanged(playerScript.health + healthModifier, playerScript.maxHealth + healthModifier);
                player.UpdateStat("increase_base_max_hp", healthModifier);
            }

            if (dashCooldownModifier != 0)
            {
                AugmentStore.Instance.DashCooldownModifier -= dashCooldownModifier;
                // playerScript.OnHealthChanged(playerScript.health + healthModifier, playerScript.maxHealth + healthModifier);
                player.UpdateStat("decrease_dash_cd", dashCooldownModifier);
            }

            if (armorModifier != 0)
            {
                AugmentStore.Instance.ArmorModifier += armorModifier;
                // player.UpdateStat("decrease_dash_cd", dashCooldownModifier);
            }

            if (moveSpeedModifer != 0)
            {
                AugmentStore.Instance.MoveSpeedModifier *= moveSpeedModifer;
                player.UpdateStat("increase_base_move_speed", moveSpeedModifer);  
            }

            if (damageModifier != 0)
            {
                AugmentStore.Instance.DamageModifier += damageModifier;
            }

            Debug.Log($"Applied {augmentName}, Health: {healthModifier}, Dash Cooldown: {dashCooldownModifier}");
        }
    }
}
