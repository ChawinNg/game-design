using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    public static Vector3 nextSpawnPosition = new Vector3(-2, -1, 0);

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
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            player.transform.position = nextSpawnPosition;
            Debug.Log("Player spawned at: " + nextSpawnPosition + " in scene: " + scene.name);
        }
        else
        {
            Debug.LogWarning("Player not found in scene: " + scene.name);
        }
    }
}