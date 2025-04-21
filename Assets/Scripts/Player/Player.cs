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
    private float baseDashCooldown = 5f;
    private bool canDash = true;
    private float dashCooldownTimer = 0f;
    public UnityAction<float, float> OnDashCooldownChanged;

    private float baseMoveSpeed = 2f;

    public float maxHealth;
    private float baseMaxHealth = 100f;

    public float health;
    public int armor = 30;
    public UnityAction<float, float> OnHealthChanged;

    public int gold = 1000;
    public float goldMult = 1;
    public UnityAction<int> OnGoldChanged;

    private BoxCollider2D playerCollider;
    private Renderer playerRenderer;

    public CombatSystem combatSystem;

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
                OnDashCooldownChanged?.Invoke(0f, dashCooldown);
                Debug.Log("Decrease Cooldown: " + dashCooldown);
                break;

            case "increase_max_hp":
                maxHealth += value;
                Debug.Log("New Max Health: " + maxHealth);
                break;

            case "gold_mult":
                float increase = goldMult * (value/100f);
                goldMult += increase;
                Debug.Log("Gold Multiplier: " + goldMult);
                break;

            case "increase_base_max_hp":
                baseMaxHealth += value;
                maxHealth = baseMaxHealth;
                health = maxHealth;
                OnHealthChanged?.Invoke(health, maxHealth);
                Debug.Log("New Base Max Health: " + baseMaxHealth);
                Debug.Log("New Max Health: " + maxHealth);
                Debug.Log("New Health: " + health);
                break;

            case "increase_base_dash_cooldown":
                baseDashCooldown -= value;
                dashCooldown = baseDashCooldown;
                Debug.Log("New Base Dash Cooldown: " + baseDashCooldown);
                Debug.Log("New Dash Cooldown: " + dashCooldown);
                break;

            case "increase_base_move_speed":
                baseMoveSpeed += value;
                moveScript.moveSpeed = baseMoveSpeed;
                Debug.Log("New Base Move Speed: " + baseMoveSpeed);
                Debug.Log("New Move Speed: " + moveScript.moveSpeed);
                break;
        }
    }

    private void Update()
    {
        StartCoroutine(Action());

        // if(combatSystem.IsPerformingAttack()) return;
        
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

    }

    void StartDash()
    {
        if (!canDash) return;
        canDash = false;
        dashCooldownTimer = dashCooldown;

        OnDashCooldownChanged?.Invoke(dashCooldownTimer, dashCooldown);

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
        OnDashCooldownChanged?.Invoke(0f, dashCooldown);
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
        animator.SetTrigger("Damage");
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

        if (Input.GetKeyDown(KeyCode.L))
        {
            AddGold(100);
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownTimer = Mathf.Max(0f, dashCooldownTimer);

            OnDashCooldownChanged?.Invoke(dashCooldownTimer, dashCooldown);
        }

        yield return null;
    }

    public void AddGold(int amount)
    {
        gold += amount * (int)Mathf.Floor(goldMult);
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
        maxHealth = baseMaxHealth;
        health = maxHealth; // Reset health to max
        armor = 30; // Reset armor (or set to any default value)
        moveScript.moveSpeed = baseMoveSpeed; // Reset movement speed (or any default value)

        // Reset any other player-related states here (such as dash cooldown, etc.)
        dashCooldown = baseDashCooldown;
        dashCooldownTimer = 0f;
        canDash = true;

        OnHealthChanged?.Invoke(health, maxHealth);

        Debug.Log("Player state reset: Health: " + health + " Armor: " + armor + " Speed: " + moveScript.moveSpeed);
        Debug.Log("Player base stat Base Health: " + baseMaxHealth + " Base Dash: " + baseDashCooldown + " Base Speed: " + baseMoveSpeed);
    }
}