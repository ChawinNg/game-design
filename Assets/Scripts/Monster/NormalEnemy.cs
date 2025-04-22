using UnityEngine;

public class NormalEnemy : Enemy
{
    public float stopDistance = 0.5f; // Minimum distance before stopping movement

    protected override void Move()
    {
        moveScript.LookInDirection(moveUp, moveDown, moveLeft, moveRight);
        agent.SetDestination(player.transform.position); 
    }
    protected override bool InAttackCondition()
    {
        return distanceToPlayer < stopDistance;
    }

    protected override void PerformAttack()
    {
        moveScript.ResetMove(); // Stop movement
        agent.ResetPath();
        if(weapon.CanPerformAttack(AttackType.Primary)){
            StartCoroutine(Attack());
        }
        
    }
}
