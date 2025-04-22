using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    protected float health;
    public int goldDropAmount;
    protected GameObject player;  // Reference to the player's Transform (position)
    public FloatingHealthBar healthBar; // Reference to the health bar UI
    public GameObject Visuals;
    protected Animator animator;
    protected SpriteRenderer enemyRenderer; // For Boss
    protected AnimationEventHandler animationEventHandler;
    protected NavMeshAgent agent; // Reference to the NavMeshAgent component
    public Weapon weapon;
    protected bool isTakingDamage = false;
    protected Move moveScript;   // Reference to the Move script (to control movement)
    protected Vector2 directionToPlayer;
    protected float distanceToPlayer;
    protected bool moveUp, moveDown, moveRight, moveLeft;
    protected bool addedGold = false;
    void Start()
    {
        health = maxHealth;

        enemyRenderer = Visuals.GetComponent<SpriteRenderer>();
        animator = Visuals.GetComponent<Animator>();
        animationEventHandler = Visuals.GetComponent<AnimationEventHandler>();

        player = GameObject.FindWithTag("Player");

        moveScript = GetComponent<Move>();
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Disable automatic rotation
        agent.updateUpAxis = false; // Disable automatic up axis adjustment
    }

    void Update()
    {
        if (weapon.IsPerformingAttack() || isTakingDamage) return;
        
        // Get the direction to the player
        directionToPlayer = (player.transform.position - transform.position).normalized;

        // Check the distance between the object and the player
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        CalMovementDir();

        if (InAttackCondition())
        {
            PerformAttack();
        }
        else
        {  
            Move();
        }
    }

    public void OnTakingDamage(float amount)
    {
        health -= amount;
        isTakingDamage = true;
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            if(!addedGold)
            {
                player.GetComponent<Player>().AddGold(goldDropAmount);
                addedGold = true;
            }
            Destroy(gameObject);
        }
        else{
            healthBar.updateHealthBar(health, maxHealth);
        }
    }

    protected IEnumerator FlashRed()
    {
        animator.SetTrigger("Damage");
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.6f);
        enemyRenderer.material.color = Color.white;
        isTakingDamage = false;
    }

    protected IEnumerator Attack()
    {
        animator.SetTrigger("Slash");
        yield return null;
    }

    protected IEnumerator RangeAttack()
    {
        animator.SetTrigger("Bow");
        yield return null;
    }

    protected virtual void CalMovementDir(){
        // Determine movement direction
        moveUp = directionToPlayer.y > 0.1f;
        moveDown = directionToPlayer.y < -0.1f;
        moveRight = directionToPlayer.x > 0.1f;
        moveLeft = directionToPlayer.x < -0.1f;
    }

    protected abstract void Move();
    protected abstract bool InAttackCondition();

    protected abstract void PerformAttack();

}
