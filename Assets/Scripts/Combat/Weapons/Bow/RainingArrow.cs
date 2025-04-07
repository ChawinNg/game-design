using System.Collections;
using UnityEngine;

public class RainingArrow : MonoBehaviour
{
    public WeaponHitbox Hitbox;
    public float Duration = 3f;
    public float baseDamage = 1f;
    public float damageInterval = 0.5f;

    void Start()
    {
        StartCoroutine(RainRoutine());
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        while (true)
        {
            GameObject[] objects = Hitbox.GetCollidingObjects();

            foreach (GameObject obj in objects)
            {
                if (!obj.CompareTag("HostileEnemy")) continue;

                obj.GetComponent<IDamageable>()?.OnTakingDamage(baseDamage);
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private IEnumerator RainRoutine()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }
}
