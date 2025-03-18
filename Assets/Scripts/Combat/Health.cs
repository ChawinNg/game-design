using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;

    private float health;

    void Start()
    {
        health = maxHealth;
    }

    public void OnTakingDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
