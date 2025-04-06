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

        float timer = 0f;

        GameController.Instance.UpdateAttackCooldown(0f, cooldown);

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            GameController.Instance.UpdateAttackCooldown(timer, cooldown);

            yield return null;
        }

        GameController.Instance.UpdateAttackCooldown(cooldown, cooldown);

        onCooldown = false;
    }
}
