using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour, IKnockbackable
{
    public float moveSpeed = 2f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool isBeingKnockback = false;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isBeingKnockback)
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed; // Apply movement
        }
    }

    public void ResetMove()
    {
        moveInput = Vector2.zero;
        animator.SetBool("Walk", false);
    }

    // Public method that other objects can call to move in a specific direction
    public void MoveInDirection(bool up, bool down, bool left, bool right)
    {
        moveInput = Vector2.zero;
        animator.SetBool("Walk", true);
        
        if (up) moveInput += Vector2.up;
        if (down) moveInput += Vector2.down;
        if (right) moveInput += Vector2.right;
        if (left) moveInput += Vector2.left;
    }

    public IEnumerator OnTakingKnockback(Vector3 force, float second)
    {
        isBeingKnockback = true;

        rb.AddForce(force / second);

        yield return new WaitForSeconds(second);

        isBeingKnockback = false;
    }
}