using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform (position)
    public Move moveScript;   // Reference to the Move script (to control movement)
    public float stopDistance = 0.5f; // Minimum distance before stopping movement

    private Vector2 directionToPlayer;  // Direction vector towards player
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Optionally, get references dynamically if not set in Inspector
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        if (moveScript == null)
        {
            moveScript = GetComponent<Move>();
        }
    }

    void Update()
    {
        // Get the direction to the player
        directionToPlayer = (player.position - transform.position).normalized;

        // Check the distance between the object and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the object is too close to the player, stop moving
        if (distanceToPlayer < stopDistance)
        {
            moveScript.ResetMove(); // Stop the movement if too close
            animator.SetTrigger("Slash");

            return;
        }

        // Get the angle between the object and the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Determine the direction based on the angle
        if (angle >= -45f && angle < 45f)
        {
            moveScript.MoveInDirection("Right");  // Move right
        }
        else if (angle >= 45f && angle < 135f)
        {
            moveScript.MoveInDirection("Up");  // Move up
        }
        else if (angle >= 135f || angle < -135f)
        {
            moveScript.MoveInDirection("Left");  // Move left
        }
        else
        {
            moveScript.MoveInDirection("Down");  // Move down
        }
    }
}
