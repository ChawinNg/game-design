using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    public Move moveScript;   // Reference to the Move script (to control movement)
    public float stopDistance = 0.5f; // Minimum distance before stopping movement

    public float startRangeAttackDistance = 3f; // Distance to start range attack
    public float stopRangeAttackDistance = 8f; // Distance to start range attack

    public float attackInterval = 3f;
    public Animator animator;
    // public UnityEvent<AttackType> OnAttack;

    public float maxHealth = 100f;

    private float health;

    private Vector2 directionToPlayer;  // Direction vector towards player
    private Transform player;  // Reference to the player's Transform (position)
    public SpriteRenderer enemyRenderer; // For Boss

    public AnimationEventHandler animationEventHandler;
    public bool isBoss = false;
    public FloatingHealthBar healthBar; // Reference to the health bar UI
    NavMeshAgent agent; // Reference to the NavMeshAgent component

    public Weapon weapon;
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        health = maxHealth;
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponent<SpriteRenderer>();
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
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false; // Disable automatic rotation
            agent.updateUpAxis = false; // Disable automatic up axis adjustment
        }
    }

    void Update()
    {
        if (weapon.IsPerformingAttack()) return;
        
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
            agent.ResetPath();
            
        
            if (weapon.CanPerformAttack())
            {
                StartCoroutine(Attack());
            } 
        }
        else  if (distanceToPlayer > startRangeAttackDistance && distanceToPlayer < stopRangeAttackDistance)
        {
            moveScript.ResetMove(); // Stop movement
            agent.ResetPath();

            if (weapon.CanPerformAttack())
            {
                StartCoroutine(RangeAttack());
            } 
        }
        else
        {  
            if (isBoss){
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
                
            }
            moveScript.LookInDirection(moveUp, moveDown, moveLeft, moveRight);
            // Move in the determined direction (supports diagonal movement)
            agent.SetDestination(player.position); // Set the destination to the player's position

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
        else{
            healthBar.updateHealthBar(health, maxHealth);
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
        yield return null;
    }

    private IEnumerator RangeAttack()
    {
        animator.SetTrigger("Bow");
        yield return null;
    }
}
