using System.Collections;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    public float cooldown = 1f;

    private bool onCooldown = false;

    protected abstract void PerformAttack();
    public abstract void UpdateAimDirection(Vector3 direction);

    public void DoPerformAttack()
    {
        if (onCooldown) return;
        PerformAttack();
        StartCoroutine(CooldownRoutine());
    }


    private IEnumerator CooldownRoutine()
    {
        onCooldown = true;

        yield return new WaitForSeconds(cooldown);

        onCooldown = false;
    }
}
