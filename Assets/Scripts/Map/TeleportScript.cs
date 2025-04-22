using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string nextScene;
    public bool isShop = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (EnemyManager.Instance != null && !EnemyManager.Instance.CanTeleport)
            return;

        string currentScene = SceneManager.GetActiveScene().name;

        if (isShop)
        {
            SceneHistory.LastScene = currentScene;
            SceneHistory.NextScene = nextScene;
            SceneManager.LoadScene("ShopMap");
        }
        else if (currentScene == "ShopMap")
        {
            SceneManager.LoadScene(SceneHistory.NextScene);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
