using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform hitbox;
    public WeaponHitbox weapon;

    public AttackBase primaryAttack;

    public void AimToDirection(Vector3 dir)
    {
        primaryAttack.UpdateAimDirection(dir);
    }

    public void PerformAttack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Primary:
                primaryAttack.DoPerformAttack();
                break;
        }
    }
}
