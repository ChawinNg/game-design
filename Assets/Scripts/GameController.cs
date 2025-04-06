using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static Vector3 nextSpawnPosition = new Vector3(-2, -1, 0);

    public Slider hpSlider;

    public Image dashCooldownImage;
    public float dashCooldownDuration = 5f;
    private bool canDash = true;
    private float dashCooldownTimer = 0f;
    public TMP_Text dashCooldownText;

    public Image attackCooldownImage;
    public TMP_Text attackCooldownText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject playerGO = GameObject.FindWithTag("Player");

        if (playerGO != null)
        {
            dashCooldownImage.fillAmount = 1f;
            playerGO.transform.position = nextSpawnPosition;

            Player player = playerGO.GetComponent<Player>();
            if (player != null)
            {
                hpSlider.maxValue = player.maxHealth;
                hpSlider.value = player.health;

                player.OnHealthChanged += OnPlayerHealthChanged;
            }

            Debug.Log("Player spawned at: " + nextSpawnPosition + " in scene: " + scene.name);
        }
        else
        {
            Debug.LogWarning("Player not found in scene: " + scene.name);
        }
    }

    private void OnPlayerHealthChanged(float current, float max)
    {
        hpSlider.maxValue = max;
        hpSlider.value = Mathf.Clamp(current, 0, max);
    }

    private void Update()
    {
        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownImage.fillAmount = (dashCooldownDuration - dashCooldownTimer) / dashCooldownDuration;
            dashCooldownImage.color = new Color(0.5f, 0.5f, 0.5f);

            dashCooldownText.text = (Mathf.Ceil(dashCooldownTimer * 10) / 10f).ToString("0.0");

            if (dashCooldownTimer <= 0f)
            {
                dashCooldownTimer = 0f;
                dashCooldownImage.fillAmount = 1f;
                dashCooldownText.text = "";
                dashCooldownImage.color = Color.white;
                canDash = true;
            }
        }
    }

    public void TriggerCooldown()
    {
        canDash = false;
        dashCooldownTimer = dashCooldownDuration;
        dashCooldownImage.fillAmount = 0f;
        dashCooldownText.text = dashCooldownDuration.ToString("0.0");
    }

    public void UpdateAttackCooldown(float timer, float attackCooldownDuration)
    {
        if (timer == attackCooldownDuration) {
            attackCooldownImage.fillAmount = 1f;
            attackCooldownText.text = "";
            attackCooldownImage.color = Color.white;
        } else {
            attackCooldownImage.fillAmount = timer / attackCooldownDuration;
            attackCooldownText.text = (Mathf.Ceil(timer * 10) / 10f).ToString("0.0");
            attackCooldownImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

}
