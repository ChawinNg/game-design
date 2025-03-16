using System.Collections.Generic;
using UnityEngine;

public class TrishulaSecondary : AttackBase
{
    [Header("Objects")]
    public GameObject Throwable;
    private List<GameObject> throwables = new();

    [Header("Stats")]
    public float ThrowableSpeed = 5f;

    private Vector3 aimmingDirection;

    public override void UpdateAimDirection(Vector3 direction)
    {
        aimmingDirection = direction;
    }
    protected override void PerformAttack()
    {
        GameObject obj = Instantiate(Throwable);
        obj.GetComponent<ThrowableTrishula>().Velocity = aimmingDirection * ThrowableSpeed;
        obj.GetComponent<Transform>().position = GetComponent<Transform>().position;

        throwables.Add(obj);
    }
}
