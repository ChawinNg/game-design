using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public Transform playerTransform;

    public Weapon currentWeapon;

    void Update()
    {
        Vector3 pos = Input.mousePosition;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(pos);
        mousePos.z = 0;

        Vector3 mouseVec = Projection.ProjectToOrthogonalSpace((mousePos - playerTransform.position).normalized);

        // currentWeapon.AimToDirection(mouseVec); 
    }

    public void PerformPrimaryAttack(AttackType attackType)
    {
        currentWeapon.PerformAttack(attackType);
    }
}
