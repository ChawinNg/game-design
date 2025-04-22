using UnityEngine;

public class SmartEnemy : Enemy
{
    public float stopDistance = 0.5f; // Minimum distance before stopping movement
    
    public float startRangeAttackDistance = 1.5f; // Distance to start range attack
    public float stopRangeAttackDistance = 6f; // Distance to start range attack


    protected override void Move()
    {
        moveScript.LookInDirection(moveUp, moveDown, moveLeft, moveRight);
        agent.SetDestination(player.transform.position); 
    }
    protected override bool InAttackCondition()
    {
        return distanceToPlayer < stopDistance || (distanceToPlayer > startRangeAttackDistance && distanceToPlayer < stopRangeAttackDistance);
    }

    protected override void PerformAttack()
    {
        moveScript.ResetMove(); // Stop movement
        agent.ResetPath();
    
        if(distanceToPlayer < stopDistance && weapon.CanPerformAttack(AttackType.Primary))
        {
            StartCoroutine(Attack());
            // Attack();
        }
        else if((distanceToPlayer > startRangeAttackDistance && distanceToPlayer < stopRangeAttackDistance) && weapon.CanPerformAttack(AttackType.Secondary))
        {
            StartCoroutine(RangeAttack());
            // RangeAttack();

        }
        
    }
}
