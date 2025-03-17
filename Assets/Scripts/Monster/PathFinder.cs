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
            Debug.Log("player found!");
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

        // If too close, stop moving and trigger attack
        if (distanceToPlayer < stopDistance)
        {
            moveScript.ResetMove(); // Stop movement
            animator.SetTrigger("Slash");
            return;
        }

        // Determine movement direction
        bool moveUp = directionToPlayer.y > 0.1f;
        bool moveDown = directionToPlayer.y < -0.1f;
        bool moveRight = directionToPlayer.x > 0.1f;
        bool moveLeft = directionToPlayer.x < -0.1f;
        Debug.Log(moveUp);


        // Move in the determined direction (supports diagonal movement)
        moveScript.MoveInDirection(moveUp, moveDown, moveLeft, moveRight);
    }
}
