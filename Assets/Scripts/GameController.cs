using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static Vector3 nextSpawnPosition = new Vector3(-2, -1, 0);

    public Slider hpSlider;
    public TMP_Text hpText;

    public Image dashCooldownImage;
    public float dashCooldownDuration = 5f;
    public TMP_Text dashCooldownText;

    public Image attackCooldownImage;
    public TMP_Text attackCooldownText;

    public TMP_Text goldText;

    public WeaponType CurrentWeapon;

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
                hpText.text = $"{player.health} / {player.maxHealth}";

                player.OnHealthChanged += OnPlayerHealthChanged;
                player.OnDashCooldownChanged += UpdateDashCooldown;
                player.OnGoldChanged += UpdateGold;
                UpdateGold(player.gold);
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

        if (hpText != null)
        {
            hpText.text = $"{current} / {max}";
        }
    }

    public void UpdateDashCooldown(float timer, float max)
    {
        if (timer <= 0f)
        {
            dashCooldownImage.fillAmount = 1f;
            dashCooldownText.text = "";
            dashCooldownImage.color = Color.white;
        }
        else
        {
            dashCooldownImage.fillAmount = (max - timer) / max;
            dashCooldownText.text = (Mathf.Ceil(timer * 10) / 10f).ToString("0.0");
            dashCooldownImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void UpdateAttackCooldown(float timer, float attackCooldownDuration)
    {
        if (timer == attackCooldownDuration)
        {
            attackCooldownImage.fillAmount = 1f;
            attackCooldownText.text = "";
            attackCooldownImage.color = Color.white;
        }
        else
        {
            attackCooldownImage.fillAmount = timer / attackCooldownDuration;
            attackCooldownText.text = (Mathf.Ceil((attackCooldownDuration - timer) * 10) / 10f).ToString("0.0");
            attackCooldownImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void UpdateGold(int gold)
    {
        if (goldText != null)
        {
            goldText.text = gold.ToString("0");
        }
    }


}
