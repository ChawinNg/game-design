using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BowPrimary : AttackBase
{
    [Header("Objects")]
    public GameObject Arrow;
    private List<GameObject> arrows = new();

    [Header("Stats")]
    public float arrowspeed = 5f;
    public float PerfectDamageMultiplier = 1.5f;
    public float PerfectTimingInSecond = 2f;
    public float PerfectWindowInSecond = 0.1f;

    public GameObject PerfectDisplay;
    public GameObject PerfectOutlineDisplay;

    private Vector3 aimmingDirection;
    private float startAttackingTime = 0f;
    private float finalSize = 0.5f;
    private bool isCharging = false;

    [Header("Events")]
    public UnityEvent onStartCharging;
    public UnityEvent onStopCharging;

    public override void UpdateAimDirection(Vector3 direction)
    {
        aimmingDirection = direction;
    }

    protected override void PerformAttack()
    {
        startAttackingTime = Time.time;
        isCharging = true;

        Debug.Log("---------- Start charging bow");
        onStartCharging?.Invoke();

        StartCoroutine(ChargeRoutine());
    }

    protected override void PostPerformAttack()
    {
        float multiplier = 1f;
        if (Math.Abs(PerfectTimingInSecond - (Time.time - startAttackingTime)) <= PerfectWindowInSecond / 2f)
        {
            multiplier = PerfectDamageMultiplier;
        }

        Shoot(multiplier);

        StopAllCoroutines();
        isCharging = false;
        startAttackingTime = 0f;
        PerfectDisplay.SetActive(false);
        PerfectOutlineDisplay.SetActive(false);
    }

    private void Shoot(float multiplier)
    {
        GameObject obj = Instantiate(Arrow);
        obj.GetComponent<Arrow>().Velocity = aimmingDirection * arrowspeed;
        obj.GetComponent<Arrow>().DamageMultiplier = multiplier;
        obj.GetComponent<Transform>().position = GetComponent<Transform>().position;

        arrows.Add(obj);

        StopHoldingAttack();

        Debug.Log("---------- Stop charging bow");
        onStopCharging?.Invoke();
    }

    private IEnumerator ChargeRoutine()
    {
        yield return new WaitForSeconds(PerfectTimingInSecond + PerfectWindowInSecond / 2.0f);
        isCharging = false;
        PerfectDisplay.SetActive(false);
        PerfectOutlineDisplay.SetActive(false);
        Shoot(1f);
    }

    private void Update()
    {
        if (isCharging)
        {
            float percent = Math.Clamp((PerfectTimingInSecond - PerfectWindowInSecond / 2f - (Time.time - startAttackingTime)) / (PerfectTimingInSecond - PerfectWindowInSecond / 2f), 0f, 1f);
            PerfectDisplay.transform.localScale = new Vector3((1f - percent) * finalSize, (1f - percent) * finalSize, 0);
            PerfectDisplay.SetActive(Time.time - startAttackingTime - PerfectTimingInSecond <= PerfectWindowInSecond / 2f);
            PerfectOutlineDisplay.SetActive(Time.time - startAttackingTime - PerfectTimingInSecond <= PerfectWindowInSecond / 2f);
        }
    }
}
