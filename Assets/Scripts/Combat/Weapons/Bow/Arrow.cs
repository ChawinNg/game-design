using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Arrow : MonoBehaviour
{
    public WeaponHitbox Hitbox;
    public Vector3 Velocity = Vector3.zero;
    public float DamageMultiplier = 1f;

    [Header("Stats")]
    public float damage = 10f;
    public float knockbackForce = 10f;
    public float knockbackTime = 0.25f;
    public float destroyInSecond = 3f;

    private HashSet<int> hitEnemy = new();

    void Start()
    {
        StartCoroutine(DestroyTime());
    }

    private IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(destroyInSecond);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        GetComponent<Transform>().Translate(Velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        GameObject[] objects = Hitbox.GetCollidingObjects();

        foreach (GameObject obj in objects)
        {
            if (!obj.CompareTag("HostileEnemy") || hitEnemy.Contains(obj.GetInstanceID())) continue;

            obj.GetComponent<IDamageable>()?.OnTakingDamage(damage * DamageMultiplier);
            IKnockbackable knockbackable = obj.GetComponent<IKnockbackable>();
            knockbackable?.TakingKnockback(Velocity.normalized * knockbackForce, knockbackTime);

            hitEnemy.Add(obj.GetInstanceID());
        }
    }
}
