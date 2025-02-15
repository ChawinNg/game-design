using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float angleDegree = 27f; // Angle in degrees, change this as needed

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Convert angle from degrees to radians
        float angleRad = angleDegree * Mathf.Deg2Rad;

        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);

        // Call this method externally to set movement direction
    }

    void FixedUpdate()
    {
        // Apply velocity to Rigidbody2D
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void ResetMove()
    {
        moveInput = Vector2.zero;
    }

    // Public method that other objects can call to move in a specific direction
    public void MoveInDirection(string direction)
    {
        // Convert angle from degrees to radians
        float angleRad = angleDegree * Mathf.Deg2Rad;

        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);

        // Check direction and adjust movement accordingly
        switch (direction)
        {
            case "Up":
                moveInput = new Vector2(x, y);  // NE
                break;
            case "Down":
                moveInput = new Vector2(-x, -y); // SW
                break;
            case "Right":
                moveInput = new Vector2(x, -y);  // SE
                break;
            case "Left":
                moveInput = new Vector2(-x, y);  // NW
                break;
            default:
                moveInput = Vector2.zero; // No movement if the direction is invalid
                break;
        }
    }
}
