using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && EnemyManager.Instance != null && EnemyManager.Instance.CanTeleport || sceneName == "FIghtMap_Grass_1")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
};