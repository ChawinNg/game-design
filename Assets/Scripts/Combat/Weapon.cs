using UnityEngine;

public class Weapon : MonoBehaviour
{
    public AttackBase primaryAttack;
    public AttackBase secondaryAttack;

    public void AimToDirection(Vector3 dir)
    {
        primaryAttack?.UpdateAimDirection(dir);
        secondaryAttack?.UpdateAimDirection(dir);
    }

    public void PerformAttack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Primary:
                primaryAttack?.DoPerformAttack();
                break;
            case AttackType.Secondary:
                secondaryAttack?.DoPerformAttack();
                break;
        }
    }
    public void PostPerformAttack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Primary:
                primaryAttack?.DoPostPerformAttack();
                break;
            case AttackType.Secondary:
                secondaryAttack?.DoPostPerformAttack();
                break;
        }
    }
}
