using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static Vector3 nextSpawnPosition = new Vector3(-2, -1, 0);

    public Slider hpSlider;

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
}
