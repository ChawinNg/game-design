using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<GameObject> activeEnemies;
    private bool isCheckingEnemies = true; 
    public GameObject AugmentSelectionUI; 


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (!isCheckingEnemies) return;

        CleanupList();
        if (activeEnemies.Count == 0)
        {
            ShowAugmentSelection();
        }
    }

     void CleanupList()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

   

    void ShowAugmentSelection()
    {
        AugmentSelectionUI.SetActive(true); 
        Time.timeScale = 0f; 
        isCheckingEnemies = false;
    }
}
