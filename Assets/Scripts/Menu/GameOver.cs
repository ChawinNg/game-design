using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    public Canvas gameOverCanvas;
    public GameObject gameOverScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    public void ShowGameOverScreen()
    {
        Debug.Log("Show Game Over screen.");
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            gameOverScreen.SetActive(true);
        }
    }

    public void HideGameOverScreen()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void PlayAgain()
    {
        Debug.Log("PlayAgain button clicked - Performing full game reset");
        
        // Hide game over UI
        HideGameOverScreen();

        GameObject playerObj = GameObject.FindWithTag("Player");
        Player player = playerObj.GetComponent<Player>();

        GameObject gameControllerObj = GameObject.Find("GameController");
        
        // Store references to our own hierarchy before destruction
        GameObject canvasObj = gameOverCanvas?.gameObject;
        GameObject screenObj = gameOverScreen?.gameObject;
        
        // Find all DontDestroyOnLoad objects and destroy them (except this one and its children)
        GameObject[] persistentObjects = FindObjectsOfType<GameObject>(true);
        
        foreach (GameObject obj in persistentObjects)
        {
            // Check if it's in the DontDestroyOnLoad scene, not this gameObject, and not a child of this gameObject
            if (obj.scene.name == "DontDestroyOnLoad" &&
                obj != this.gameObject &&
                obj != canvasObj &&
                obj != screenObj &&
                obj != playerObj &&
                obj != gameControllerObj &&
                !obj.transform.IsChildOf(this.transform) &&
                !obj.transform.IsChildOf(playerObj.transform) &&
                !obj.transform.IsChildOf(gameControllerObj.transform))
            {
                Debug.Log($"Cleaning up persistent object: {obj.name}");
                Destroy(obj);
            }
        }
        
        // Reset time scale (in case it was modified)
        Time.timeScale = 1.0f;
        
        // Load the first scene in build settings
        SceneManager.LoadScene("HomeMapV2");
        player.ResetHealth();
        // We don't destroy this object, so it stays with all its children intact
        // This is important if you want to keep the GameOver hierarchy available
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
