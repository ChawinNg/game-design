using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public Transform playerTransform;

    public Weapon Trishula;
    public Weapon Club;
    public Weapon Bow;

    private Weapon GetCurrentWeapon()
    {
        return GameController.Instance.CurrentWeapon switch
        {
            WeaponType.Trishula => Trishula,
            WeaponType.Club => Club,
            WeaponType.Bow => Bow,
            _ => null,
        };
    }

    void Update()
    {
        Vector3 pos = Input.mousePosition;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(pos);
        mousePos.z = 0;

        Vector3 mouseVec = Projection.ProjectToOrthogonalSpace((mousePos - playerTransform.position).normalized);

        GetCurrentWeapon().AimToDirection(mouseVec);
    }

    public void PerformAttack(AttackType attackType)
    {
        GetCurrentWeapon().PerformAttack(attackType);
    }

    public void PostPerformAttack(AttackType attackType)
    {
        GetCurrentWeapon().PostPerformAttack(attackType);
    }
}
