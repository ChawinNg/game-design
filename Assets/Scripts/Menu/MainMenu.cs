using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Replace "GameScene" with the actual name of your game scene
        SceneManager.LoadScene("HomeMapV2");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!"); // For testing in Unity Editor
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
