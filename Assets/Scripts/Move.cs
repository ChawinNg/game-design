using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour, IKnockbackable
{
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool isBeingKnockback = false;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
        moveInput = Vector2.zero; // If don't set, obj will move like it is on ice

        if (right)
        {
            animator.SetFloat("x", 1f);
            animator.SetFloat("y", 0f);
            animator.SetBool("Walk", true);
            moveInput += Vector2.right;

            if (up)
            {
                moveInput += Vector2.up;
            }
            if (down)
            {
                moveInput += Vector2.down;
            }
        }
        else if (left)
        {
            animator.SetFloat("x", -1f);
            animator.SetFloat("y", 0f);
            animator.SetBool("Walk", true);
            moveInput += Vector2.left;
            if (up)
            {
                moveInput += Vector2.up;
            }
            if (down)
            {
                moveInput += Vector2.down;
            }
        }
        else
        {
            animator.SetFloat("x", 0f);

            if (up)
            {
                animator.SetFloat("y", 1f);
                moveInput += Vector2.up;
                animator.SetBool("Walk", true);
            }
            if (down)
            {
                animator.SetFloat("y", -1f);
                moveInput += Vector2.down;
                animator.SetBool("Walk", true);
            }
        }
    }

    public void TakingKnockback(Vector3 force, float second)
    {
        isBeingKnockback = true;
        // Debug.Log("start");
        ResetMove();

        StartCoroutine(OnTakingKnockback(force, second));
    }

    public IEnumerator OnTakingKnockback(Vector3 force, float second)
    {
        rb.AddForce(force / second);

        yield return new WaitForSeconds(second);
        isBeingKnockback = false;
    }
}