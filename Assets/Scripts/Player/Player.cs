using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    public UnityEvent<AttackType> OnAttack, OnPostAttack;
    public Move moveScript;

    Animator animator;

    public float dashSpeedMultiplier = 3f; // Dash is 3x normal speed
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    private bool canDash = true;
    private float dashCooldownTimer = 0f;

    public float maxHealth;

    public float health;
    public int armor = 30;
    public UnityAction<float, float> OnHealthChanged;

    public int gold = 0;
    public UnityAction<int> OnGoldChanged;

    private BoxCollider2D playerCollider;
    private Renderer playerRenderer;

    void Awake()
    {
        if (GameObject.FindWithTag("Player") != null && GameObject.FindWithTag("Player") != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("No BoxCollider2D found on the Player!");
        }

        playerRenderer = GetComponent<Renderer>();

        if (AugmentStore.Instance != null)
        {
            AugmentStore.Instance.OnStatChanged += UpdateStat;
            UpdateStat(nameof(AugmentStore.DamageModifier), AugmentStore.Instance.DamageModifier);
        }
    }

    public void UpdateStat(string statName, float value)
    {
        switch (statName)
        {
            case nameof(AugmentStore.HealthModifier):
                maxHealth = 100f + value;
                Debug.Log("Updated Health: " + maxHealth);
                break;

            case nameof(AugmentStore.MoveSpeedModifier):
                moveScript.moveSpeed = 2f + value;
                Debug.Log("Updated Move Speed: " + moveScript.moveSpeed);
                break;

            case nameof(AugmentStore.ArmorModifier):
                armor = 10 + (int)value;
                Debug.Log("Updated Armor: " + armor);
                break;

            case "heal":
                float healAmount = maxHealth * (value / 100f);
                health = Mathf.Min(health + healAmount, maxHealth);
                OnHealthChanged?.Invoke(health, maxHealth);
                Debug.Log("Healed: " + healAmount + ", New Health: " + health);
                break;

            case "add_move_speed":
                float addSpeed = moveScript.moveSpeed * (value / 100f);
                moveScript.moveSpeed += addSpeed;
                Debug.Log("Updated Move Speed: " + moveScript.moveSpeed);
                break;

            case "decrease_dash_cd":
                float dec_cd = dashCooldown * (value / 100f);
                dashCooldown -= dec_cd;
                Debug.Log("Decrease Cooldown: " + dashCooldown);
                break;
        }
    }

    private void Update()
    {
        // Get movement input for all directions
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);

        if (moveUp || moveDown || moveLeft || moveRight)
        {
            // Move in the determined direction (supports diagonal movement)
            moveScript.MoveInDirection(moveUp, moveDown, moveLeft, moveRight);
        }
        else
        {
            moveScript.ResetMove();
        }

        StartCoroutine(Action());
        UpdateUI();
    }

    void StartDash()
    {
        if (!canDash) return;

        canDash = false;
        dashCooldownTimer = dashCooldown;
        GameController.Instance.TriggerCooldown();
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
        // healthText.text = "Health: " + health.ToString();
        // armorText.text = "Armor: " + armor.ToString();
    }

    void Attack()
    {
        Debug.Log("Attack triggered!" + " Health: " + health + " Armor: " + armor + " Dash Cooldown: " + dashCooldown + "Move Speed: " + moveScript.moveSpeed);
        OnAttack?.Invoke(AttackType.Primary);
    }

    void PostAttack()
    {
        Debug.Log("Post attack triggered");
        OnPostAttack?.Invoke(AttackType.Primary);
    }

    void UseWeaponSkill()
    {
        Debug.Log("Weapon skill activated!");
        OnAttack?.Invoke(AttackType.Secondary);
    }

    void PostUseWeaponSkill()
    {
        Debug.Log("Post Weapon skill triggered!");
        OnPostAttack?.Invoke(AttackType.Secondary);
    }

    public void OnTakingDamage(float amount)
    {
        health -= amount;
        OnHealthChanged?.Invoke(health, maxHealth);

        UpdateUI();
        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (GameOver.Instance != null)
        {
            GameOver.Instance.ShowGameOverScreen();
        }
    }

    private IEnumerator FlashRed()
    {
        playerRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        playerRenderer.material.color = Color.white;
    }

    private IEnumerator Action()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartDash();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (ShopTrigger.IsShopOpen) yield break;
            animator.SetTrigger("Slash");
            Attack();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (ShopTrigger.IsShopOpen) yield break;
            PostAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            UseWeaponSkill();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            PostUseWeaponSkill();
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        yield return null;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
        Debug.Log("Gold increased. Current gold: " + gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            OnGoldChanged?.Invoke(gold);
            Debug.Log("Gold spent: " + amount + ". Current gold: " + gold);
            return true;
        }
        else
        {
            Debug.Log("Not enough gold to spend. Current gold: " + gold);
            return false;
        }
    }

    // This function resets the player's state when the game starts or when Play Again is clicked
    public void ResetPlayer()
    {
        health = maxHealth; // Reset health to max
        armor = 30; // Reset armor (or set to any default value)
        moveScript.moveSpeed = 2f; // Reset movement speed (or any default value)

        // Reset any other player-related states here (such as dash cooldown, etc.)
        dashCooldownTimer = 0f;
        canDash = true;

        OnHealthChanged?.Invoke(health, maxHealth);

        Debug.Log("Player state reset: Health: " + health + " Armor: " + armor + " Speed: " + moveScript.moveSpeed);
    }
}
