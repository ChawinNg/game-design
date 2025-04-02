using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ClubSecondary : AttackBase
{
    [Header("Objects")]
    public WeaponHitbox hitbox;
    public Transform transform;

    [Header("Stats")]
    public float baseDamage = 10f;

    public float knockbackForce = 10f;
    public float knockbackTime = 0.25f;

    public float startDamageRate = 0.8f;
    public float endDamageRate = 4.0f;

    public string attackabletag = "HostileEnemy";
    public float attackInterval = 0.5f;
    public float spinDuration = 5f;
    private bool spinning = false;
    private float startSpinningTime = 0f;

    public override void UpdateAimDirection(Vector3 direction) { }

    protected override void PerformAttack()
    {
        spinning = true;
        startSpinningTime = Time.time;

        StartCoroutine(SpinRoutine());
        StartCoroutine(SpinPerformAttack());
    }

    private IEnumerator SpinRoutine()
    {
        hitbox.gameObject.SetActive(true);

        yield return new WaitForSeconds(spinDuration);

        hitbox.gameObject.SetActive(false);
        spinning = false;
    }

    private IEnumerator SpinPerformAttack()
    {
        while (spinning)
        {
            float rate = startDamageRate + (endDamageRate - startDamageRate) * (Time.time - startSpinningTime);
            float damage = baseDamage * rate;

            GameObject[] objects = hitbox.GetCollidingObjects();

            foreach (GameObject obj in objects)
            {
                if (!obj.CompareTag(attackabletag)) continue;

                Vector3 direction = (obj.GetComponent<Transform>().position - transform.position).normalized;


                obj.GetComponent<IDamageable>()?.OnTakingDamage(damage);
                IKnockbackable knockbackable = obj.GetComponent<IKnockbackable>();
                knockbackable?.TakingKnockback(direction * knockbackForce, knockbackTime);
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }
}
