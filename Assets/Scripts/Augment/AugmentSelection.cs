using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class AugmentSelection : MonoBehaviour
{
    public GameObject augmentButtonPrefab;
    public Transform augmentContainer;

    public GameObject AugmentSelectionUI; 
    public List<AugmentData> availableAugments;
    public Transform selectedAugmentsPanel;
    public GameObject augmentSlotPrefab;
    void Start()
    {
        PopulateAugments();
        AugmentSelectionUI.SetActive(false);
    }

    void PopulateAugments()
    {
        List<AugmentData> chosenAugments = availableAugments.OrderBy(x => Random.value).Take(3).ToList();
        foreach (Transform child in augmentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var augment in chosenAugments)
        {
            GameObject buttonObj = Instantiate(augmentButtonPrefab, augmentContainer);
            buttonObj.GetComponentInChildren<TMP_Text>().text = augment.augmentName;
            var iconImage = buttonObj.transform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = augment.iconSprite;
            iconImage.SetNativeSize(); 
            Debug.Log($"augmentName Type: {augment.augmentName.GetType()} - Value: {augment.augmentName}");


            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectAugment(augment));
        }
    }

    void SelectAugment(AugmentData augment)
    {
        augment.ApplyEffect();
        DontDestroyOnLoad(selectedAugmentsPanel);
        GameObject newSlot = Instantiate(augmentSlotPrefab, selectedAugmentsPanel);
        var iconImage = newSlot.transform.Find("Icon").GetComponentInChildren<Image>();
        iconImage.sprite = augment.iconSprite;
        iconImage.SetNativeSize(); 
        Time.timeScale = 1f; 
        AugmentSelectionUI.SetActive(false);
    }
}
