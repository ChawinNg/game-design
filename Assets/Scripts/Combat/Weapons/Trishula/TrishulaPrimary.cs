using System.Data.Common;
using UnityEngine;

public class TrishulaPrimary : AttackBase
{
    [Header("Objects")]
    public WeaponHitbox hitbox;
    private Transform hitboxTransform;

    [Header("Stats")]
    public float baseDamage = 10f;
    public float knockbackForce = 10f;
    public float knockbackTime = 0.25f;
    public string attackabletag = "HostileEnemy";

    private float attackRange;

    private Vector3 aimmingDirection;

    void Start()
    {
        hitboxTransform = hitbox.GetComponent<Transform>();
        attackRange = hitboxTransform.localPosition.magnitude;

        if (AugmentStore.Instance != null)
        {
            AugmentStore.Instance.OnStatChanged += UpdateStat;
            UpdateStat(nameof(AugmentStore.DamageModifier), AugmentStore.Instance.DamageModifier);
        }
    }
    private void OnDestroy()
    {
        if (AugmentStore.Instance != null)
        {
            AugmentStore.Instance.OnStatChanged -= UpdateStat;
        }
    }
    private void UpdateStat(string statName, float value)
    {
        if (statName == nameof(AugmentStore.DamageModifier))
        {
            baseDamage = 10f + 10f * value;
            Debug.Log("Updated Base Damage: " + baseDamage);
        }
    }
    public override void UpdateAimDirection(Vector3 direction)
    {
        aimmingDirection = direction;
        hitboxTransform.localPosition = direction * attackRange;
        // Debug.Log($"Hitbox Position: {hitboxTransform.position}");
    }

    protected override void PerformAttack()
    {
        GameObject[] objects = hitbox.GetCollidingObjects();

        foreach (GameObject obj in objects)
        {
            if (!obj.CompareTag(attackabletag)) continue;
            
            obj.GetComponent<IDamageable>()?.OnTakingDamage(baseDamage);
            IKnockbackable knockbackable = obj.GetComponent<IKnockbackable>();
            if (knockbackable != null)
            {
                knockbackable.TakingKnockback(aimmingDirection * knockbackForce, knockbackTime);
            }
        }
    }

    protected override void PostPerformAttack() { }
}
