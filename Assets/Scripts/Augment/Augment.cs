using UnityEngine;

[CreateAssetMenu(fileName = "New Augment", menuName = "Game/Augment")]
public class AugmentData : ScriptableObject
{
    public string augmentName;
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
        Move moveScript = player.moveScript;
        AugmentSelection augementSelection = FindFirstObjectByType<AugmentSelection>();
        if (playerScript != null)
        {
            if (healthModifier != 0) 
            {
                playerScript.OnTakingDamage(-healthModifier);
            }

            if (dashCooldownModifier != 0)
            {
                playerScript.dashCooldown -= dashCooldownModifier;
            }

            if (armorModifier != 0)
            {
                playerScript.armor += armorModifier;
            }

            if (moveSpeedModifer != 0) {
                moveScript.moveSpeed *= moveSpeedModifer;
            }

            if (damageModifier != 0) {
                augementSelection.damageModifier += damageModifier;
            }

            Debug.Log($"Applied {augmentName}, Health: {healthModifier}, Dash Cooldown: {dashCooldownModifier}");
        }
    }
}
