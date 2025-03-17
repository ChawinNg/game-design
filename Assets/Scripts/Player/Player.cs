using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public UnityEvent<AttackType> OnAttack;
    public Move moveScript;
    Animator animator;

    public float dashSpeedMultiplier = 3f; // Dash is 3x normal speed
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    private bool canDash = true;
    private float dashCooldownTimer = 0f;

    public int health = 100;
    public int armor = 30;

    private BoxCollider2D playerCollider;
    private Renderer playerRenderer;
    public Text cooldownText;
    public Text healthText;
    public Text armorText;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("No BoxCollider2D found on the Player!");
        }

        playerRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        moveScript.ResetMove();

        // Get movement input for all directions
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);

        // Move in the determined direction (supports diagonal movement)
        moveScript.MoveInDirection(moveUp, moveDown, moveLeft, moveRight);

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartDash();
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Slash");
            Attack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            UseWeaponSkill();
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            cooldownText.text = "Dash Cooldown: " + Mathf.Ceil(dashCooldownTimer).ToString() + "s";
        }
        else
        {
            cooldownText.text = "Dash Ready!";
        }

        UpdateUI();
    }

    void StartDash()
    {
        if (!canDash) return;

        canDash = false;
        dashCooldownTimer = dashCooldown;

        // **Increase Move Speed for Dash**
        moveScript.moveSpeed *= dashSpeedMultiplier;

        Invoke(nameof(EndDash), dashDuration);
        Invoke(nameof(ResetDash), dashCooldown);
    }

    void EndDash()
    {
        moveScript.moveSpeed /= dashSpeedMultiplier; // Reset speed after dash
    }

    void ResetDash()
    {
        canDash = true;
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + health.ToString();
        armorText.text = "Armor: " + armor.ToString();
    }

    void Attack()
    {
        Debug.Log("Attack triggered!");
        OnAttack?.Invoke(AttackType.Primary);
    }

    void UseWeaponSkill()
    {
        Debug.Log("Weapon skill activated!");
        OnAttack?.Invoke(AttackType.Secondary);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        UpdateUI();
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        playerRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        playerRenderer.material.color = Color.white;
    }
}
