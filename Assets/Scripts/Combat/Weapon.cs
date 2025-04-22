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

    public bool CanPerformAttack(AttackType type){
        switch (type)
        {
            case AttackType.Primary:
                return primaryAttack?.CanPerformAttack() ?? false;
            case AttackType.Secondary:
                return secondaryAttack?.CanPerformAttack() ?? false;
            default:
                return false;
        }
    }

    public bool IsPerformingAttack(){
        bool IsPerforming = primaryAttack != null && secondaryAttack != null && (primaryAttack.IsPerformingAttack() || secondaryAttack.IsPerformingAttack());
        return IsPerforming;
    }
}
