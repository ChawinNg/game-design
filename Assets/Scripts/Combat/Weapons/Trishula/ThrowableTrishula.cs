using UnityEngine;

[RequireComponent(typeof(Transform))]
public class ThrowableTrishula : MonoBehaviour
{
    public WeaponHitbox Hitbox;
    public Vector3 Velocity = Vector3.zero;

    [Header("Stats")]
    public float baseDamage = 10f;
    public float knockbackForce = 10f;
    public float knockbackTime = 0.25f;

    public string attackabletag = "HostileEnemy";

    void FixedUpdate()
    {
        GetComponent<Transform>().Translate(Velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        GameObject[] objects = Hitbox.GetCollidingObjects();

        bool isHit = false;

        foreach (GameObject obj in objects)
        {
            if (!obj.CompareTag(attackabletag)) continue;

            isHit = true;

            obj.GetComponent<IDamageable>()?.OnTakingDamage(baseDamage);
            IKnockbackable knockbackable = obj.GetComponent<IKnockbackable>();
            if (knockbackable != null)
            {
                knockbackable.TakingKnockback(Velocity.normalized * knockbackForce, knockbackTime);
            }
        }

        if (isHit)
        {
            Destroy(gameObject);
        }
    }
}
