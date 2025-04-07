using System.Collections.Generic;
using UnityEngine;

public class BowSecondary : AttackBase
{
    [Header("Objects")]
    public GameObject Rain;
    private List<GameObject> throwables = new();

    [Header("Stats")]
    public float baseDamage = 10f;

    public override void UpdateAimDirection(Vector3 direction) { }

    protected override void PerformAttack()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        GameObject rain = Instantiate(Rain);
        rain.GetComponent<Transform>().position = new Vector3(worldPos.x, worldPos.y, 0);
        throwables.Add(rain);
    }

    protected override void PostPerformAttack() { }
}
