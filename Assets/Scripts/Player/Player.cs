using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Move moveScript;
    
    public float dashSpeedMultiplier = 3f; // Dash is 3x normal speed
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    private bool canDash = true;
    private float dashCooldownTimer = 0f;

    public int health = 100;
    public int armor = 30;

    private CapsuleCollider2D playerCollider;
    private Renderer playerRenderer;
    public Text cooldownText;
    public Text healthText;
    public Text armorText;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("No CapsuleCollider2D found on the Player!");
        }

        playerRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        moveScript.ResetMove();

        // Example of controlling movement externally
        if (Input.GetKey(KeyCode.W)) 
        {
            moveScript.MoveInDirection("Up");
        }
        else if (Input.GetKey(KeyCode.S)) 
        {
            moveScript.MoveInDirection("Down");
        }
        else if (Input.GetKey(KeyCode.D)) 
        {
            moveScript.MoveInDirection("Right");
        }
        else if (Input.GetKey(KeyCode.A)) 
        {
            moveScript.MoveInDirection("Left");
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartDash();
        }

        if (Input.GetMouseButtonDown(0))
        {
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
    }

    void UseWeaponSkill()
    {
        Debug.Log("Weapon skill activated!");
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
