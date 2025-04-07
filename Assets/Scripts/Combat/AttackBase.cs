using System.Collections;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    public float cooldown = 1f;

    private bool onCooldown = false;
    private bool holding = false;

    protected abstract void PerformAttack();
    protected abstract void PostPerformAttack();
    public abstract void UpdateAimDirection(Vector3 direction);

    public void DoPerformAttack()
    {
        if (onCooldown) return;
        PerformAttack();
        holding = true;
    }

    public void DoPostPerformAttack()
    {
        if (!holding) return;
        PostPerformAttack();
        StopHoldingAttack();
    }

    protected void StopHoldingAttack()
    {
        StartCoroutine(CooldownRoutine());
        holding = false;
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
