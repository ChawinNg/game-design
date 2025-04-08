using UnityEngine;

public class MonsterCombatSystem : MonoBehaviour
{
    public Transform monsterTransform;
    public Transform playerTransform;

    public Weapon currentWeapon;

    void Start()
    {
        // Optionally, get references dynamically if not set in Inspector
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
    }
    void Update()
    {
        // Debug.Log($"Player Position: {playerTransform.position}, Monster Position: {monsterTransform.position}");
        Vector3 mouseVec = Projection.ProjectToOrthogonalSpace((playerTransform.position - monsterTransform.position).normalized);
        // Vector3 mouseVec = (playerTransform.position - monsterTransform.position).normalized;
        currentWeapon.AimToDirection(mouseVec);
    }

    public void PerformAttack(AttackType attackType)
    {
        Debug.Log("attacking!");
        currentWeapon.PerformAttack(attackType);
    }
}
