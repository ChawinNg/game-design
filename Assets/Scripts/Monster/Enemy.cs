using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    public Move moveScript;   // Reference to the Move script (to control movement)
    public float stopDistance = 0.5f; // Minimum distance before stopping movement
    public float attackInterval = 3f;
    public Animator animator;
    public UnityEvent<AttackType> OnAttack;

    public float maxHealth = 100f;

    private float health;

    private Vector2 directionToPlayer;  // Direction vector towards player
    private Transform player;  // Reference to the player's Transform (position)
    private float lastAttackTime = 0f;
    public Renderer enemyRenderer;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        health = maxHealth;
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponent<Renderer>();
        }
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

        

        // Determine movement direction
        bool moveUp = directionToPlayer.y > 0.1f;
        bool moveDown = directionToPlayer.y < -0.1f;
        bool moveRight = directionToPlayer.x > 0.1f;
        bool moveLeft = directionToPlayer.x < -0.1f;

        

        if (distanceToPlayer < stopDistance)
        {
            moveScript.ResetMove(); // Stop movement

            if (Time.time - lastAttackTime >= attackInterval)
            {
                StartCoroutine(Attack());
           
            }
        }
        else
        {
            // Move in the determined direction (supports diagonal movement)
            moveScript.MoveInDirection(moveUp, moveDown, moveLeft, moveRight);
        }
    }

    public void OnTakingDamage(float amount)
    {
        health -= amount;
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        enemyRenderer.material.color = Color.white;
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Slash");
        OnAttack?.Invoke(AttackType.Primary);
        lastAttackTime = Time.time; // Update last attack time
        yield return null;
    }
}
