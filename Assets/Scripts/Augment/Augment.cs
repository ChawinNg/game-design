using UnityEngine;

[CreateAssetMenu(fileName = "New Augment", menuName = "Game/Augment")]
public class AugmentData : ScriptableObject
{
    public string augmentName;
    public Sprite iconSprite;
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
                AugmentStore.Instance.HealthModifier += healthModifier; ;
            }

            if (dashCooldownModifier != 0)
            {
                AugmentStore.Instance.DashCooldownModifier -= dashCooldownModifier;
            }

            if (armorModifier != 0)
            {
                AugmentStore.Instance.ArmorModifier += armorModifier;
            }

            if (moveSpeedModifer != 0)
            {
                AugmentStore.Instance.MoveSpeedModifier *= moveSpeedModifer;
            }

            if (damageModifier != 0)
            {
                AugmentStore.Instance.DamageModifier += damageModifier;
            }

            Debug.Log($"Applied {augmentName}, Health: {healthModifier}, Dash Cooldown: {dashCooldownModifier}");
        }
    }
}
