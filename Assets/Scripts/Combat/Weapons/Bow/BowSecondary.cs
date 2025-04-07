using UnityEngine;

public class BowSecondary : AttackBase
{
    public GameObject obj;

    [Header("Stats")]
    public float baseDamage = 10f;

    public string attackabletag = "HostileEnemy";

    public override void UpdateAimDirection(Vector3 direction) { }

    protected override void PerformAttack()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Instantiate(obj, worldPos, Quaternion.Euler(0, 0, 0));
    }

    protected override void PostPerformAttack() { }
}
