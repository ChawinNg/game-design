using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public WeaponHitbox Hitbox;
    public float baseDamage = 1f;

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
                if (!obj.CompareTag("Player")) continue;

                obj.GetComponent<IDamageable>()?.OnTakingDamage(baseDamage);
            }
            yield return new WaitForSeconds(0.2f);

        }
    }

    private IEnumerator RainRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
