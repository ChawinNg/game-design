using UnityEngine;

public class RangeEnemy : Enemy
{
    private float startRangeAttackDistance = 1.5f; // Distance to start range attack
    private float stopRangeAttackDistance = 1000f; // Distance to start range attack

    protected override void CalMovementDir(){
        // Determine movement direction
        moveUp = directionToPlayer.y > -startRangeAttackDistance;
        moveDown = directionToPlayer.y < startRangeAttackDistance;
        moveRight = directionToPlayer.x > -startRangeAttackDistance;
        moveLeft = directionToPlayer.x < startRangeAttackDistance;
    }
    protected override void Move()
    {
        moveScript.LookInDirection(moveUp, moveDown, moveLeft, moveRight);
        // agent.SetDestination(player.transform.position); //!fix this
        // Calculate a position at the edge of the attack range
        Vector3 directionToPlayer3D = (player.transform.position - transform.position).normalized;
        Vector3 targetPosition = player.transform.position - directionToPlayer3D * startRangeAttackDistance;

        // Check if the calculated position is reachable
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(targetPosition, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // Set the destination to the valid position
        }
        else
        {
            agent.SetDestination(transform.position); // Stay in place if no valid position is found
        }
    }
    protected override bool InAttackCondition()
    {
        return distanceToPlayer > startRangeAttackDistance && distanceToPlayer < stopRangeAttackDistance;
    }

    protected override void PerformAttack()
    {
        moveScript.ResetMove(); // Stop movement
        agent.ResetPath();
        
        if (weapon.CanPerformAttack())
        {
            StartCoroutine(RangeAttack());
        } 
    }
}
