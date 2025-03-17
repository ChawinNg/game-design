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

    private float attackRange;

    private Vector3 aimmingDirection;

    void Start()
    {
        hitboxTransform = hitbox.GetComponent<Transform>();
        attackRange = hitboxTransform.localPosition.magnitude;
    }
    public override void UpdateAimDirection(Vector3 direction)
    {
        aimmingDirection = direction;
        hitboxTransform.localPosition = direction * attackRange;
    }

    protected override void PerformAttack()
    {
        GameObject[] objects = hitbox.GetCollidingObjects();

        foreach (GameObject obj in objects)
        {
            if (!obj.CompareTag("HostileEnemy")) continue;

            obj.GetComponent<IDamageable>()?.OnTakingDamage(baseDamage);
            IKnockbackable knockbackable = obj.GetComponent<IKnockbackable>();
            if (knockbackable != null)
            {
                StartCoroutine(knockbackable.OnTakingKnockback(aimmingDirection * knockbackForce, knockbackTime));
            }
        }
    }
}
