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

    void FixedUpdate()
    {
        GetComponent<Transform>().Translate(Velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        GameObject[] objects = Hitbox.GetCollidingObjects();

        foreach (GameObject obj in objects)
        {
            if (!obj.CompareTag("HostileEnemy")) continue;

            obj.GetComponent<IDamageable>()?.OnTakingDamage(baseDamage);
            IKnockbackable knockbackable = obj.GetComponent<IKnockbackable>();
            if (knockbackable != null)
            {
                StartCoroutine(knockbackable.OnTakingKnockback(Velocity.normalized * knockbackForce, knockbackTime));
            }
        }

        if (objects.Length > 0)
        {
            Destroy(gameObject);
        }
    }
}
