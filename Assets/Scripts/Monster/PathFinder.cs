using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform (position)
    public Move moveScript;   // Reference to the Move script (to control movement)

    private Vector2 directionToPlayer;  // Direction vector towards player

    void Start()
    {
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
        // Calculate direction towards the player
        directionToPlayer = (player.position - transform.position).normalized;

        // Calculate the angle to move towards the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Convert angle to a direction like "Up", "Down", "Left", or "Right"
        if (angle >= -45f && angle < 45f)
        {
            moveScript.MoveInDirection("Right");
        }
        else if (angle >= 45f && angle < 135f)
        {
            moveScript.MoveInDirection("Up");
        }
        else if (angle >= 135f || angle < -135f)
        {
            moveScript.MoveInDirection("Left");
        }
        else
        {
            moveScript.MoveInDirection("Down");
        }
    }
}