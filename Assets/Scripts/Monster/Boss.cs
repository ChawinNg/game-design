using UnityEngine;

public class Boss : Enemy
{
    public float stopDistance = 0.5f; // Minimum distance before stopping movement
    
    public float startRangeAttackDistance = 1.5f; // Distance to start range attack
    public float stopRangeAttackDistance = 6f; // Distance to start range attack
    protected override void Move()
    {
        if(!enemyRenderer.flipX && moveRight){
            enemyRenderer.flipX = true;
            Vector3 spriteLocalPosition = enemyRenderer.transform.localPosition;
            spriteLocalPosition.x = -spriteLocalPosition.x; 
            enemyRenderer.transform.localPosition = spriteLocalPosition;
        }
        else if(enemyRenderer.flipX && moveLeft){
            enemyRenderer.flipX = false;
            Vector3 spriteLocalPosition = enemyRenderer.transform.localPosition;
            spriteLocalPosition.x = -spriteLocalPosition.x; 
            enemyRenderer.transform.localPosition = spriteLocalPosition;
        }
        moveScript.LookInDirection(moveUp, moveDown, moveLeft, moveRight);
        // Move in the determined direction (supports diagonal movement)
        agent.SetDestination(player.transform.position); // Set the destination to the player's position
    }
    protected override bool InAttackCondition()
    {
        return distanceToPlayer < stopDistance || (distanceToPlayer > startRangeAttackDistance && distanceToPlayer < stopRangeAttackDistance);
    }

    protected override void PerformAttack()
    {
        moveScript.ResetMove(); // Stop movement
        agent.ResetPath();
    
        if (weapon.CanPerformAttack())
        {
            if(distanceToPlayer < stopDistance)
            {
                StartCoroutine(Attack());
            }
            else
            {
                StartCoroutine(RangeAttack());
            }
        } 
    }
}