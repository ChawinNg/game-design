using System.Collections.Generic;
using UnityEngine;

public class BossSecondary : AttackBase
{
    [Header("Objects")]
    public GameObject Spell;
    private List<GameObject> throwables = new();

    [Header("Stats")]
    public float baseDamage = 10f;

    private Transform player; 

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    public override void UpdateAimDirection(Vector3 direction) { }

    protected override void PerformAttack()
    {
        // Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        GameObject spell = Instantiate(Spell);
        spell.GetComponent<Transform>().position = new Vector3(player.position.x, player.position.y, 0);
        throwables.Add(spell);
    }

    protected override void PostPerformAttack() { }
}
