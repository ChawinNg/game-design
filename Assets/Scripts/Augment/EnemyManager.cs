using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<GameObject> activeEnemies;
    private bool isCheckingEnemies = true; 
    public bool CanTeleport { get; private set; } = false;

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
            CanTeleport = true;
            ShowAugmentSelection();
        }
        else { 
            CanTeleport = false;  
        }
    }

     void CleanupList()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

   

    void ShowAugmentSelection()
    {
        if (AugmentSelection.Instance != null)
        {
            AugmentSelection.Instance.AugmentSelectionUI.SetActive(true);
            Time.timeScale = 0f;
            isCheckingEnemies = false;
        }
        else
        {
            Debug.LogWarning("AugmentSelection.Instance is null — could not show UI.");
        }
    }

}
