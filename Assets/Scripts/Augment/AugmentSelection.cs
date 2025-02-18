using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AugmentSelection : MonoBehaviour
{
    public GameObject augmentButtonPrefab;
    public Transform augmentContainer;

    public GameObject AugmentSelectionUI; 
    public List<AugmentData> availableAugments;
    public GameObject player;
    void Start()
    {
        PopulateAugments();
        AugmentSelectionUI.SetActive(false);
    }

    void PopulateAugments()
    {
        foreach (var augment in availableAugments)
        {
            GameObject buttonObj = Instantiate(augmentButtonPrefab, augmentContainer);
            buttonObj.GetComponentInChildren<TMP_Text>().text = augment.augmentName;
            Debug.Log($"augmentName Type: {augment.augmentName.GetType()} - Value: {augment.augmentName}");


            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectAugment(augment));
        }
    }

    void SelectAugment(AugmentData augment)
    {
        augment.ApplyEffect();
        Time.timeScale = 1f; 
        AugmentSelectionUI.SetActive(false);
    }
}
