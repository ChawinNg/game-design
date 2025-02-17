using UnityEngine;

[CreateAssetMenu(fileName = "New Augment", menuName = "Game/Augment")]
public class AugmentData : ScriptableObject
{
    public string augmentName;
    public string description;
    public int healthModifier; // For health-related changes
    public float damageModifier;
    public float dashCooldownModifier;
     public void ApplyEffect()
    {
        Player player = FindFirstObjectByType<Player>();
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            if (healthModifier != 0) 
            {
                playerScript.TakeDamage(-healthModifier);
            }

            if (dashCooldownModifier != 0)
            {
                playerScript.dashCooldown -= dashCooldownModifier;
            }

            Debug.Log($"Applied {augmentName}, Health: {healthModifier}, Dash Cooldown: {dashCooldownModifier}");
        }
    }
}
