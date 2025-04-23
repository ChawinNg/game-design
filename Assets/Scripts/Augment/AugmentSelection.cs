using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AugmentSelection : MonoBehaviour
{
    public static AugmentSelection Instance { get; private set; }
    public GameObject augmentButtonPrefab;
    public Transform augmentContainer;

    public GameObject AugmentSelectionUI; 
    public List<AugmentData> availableAugments;
    public Transform selectedAugmentsPanel;
    public GameObject augmentSlotPrefab;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }    
    void Start()
    {
        PopulateAugments();
        AugmentSelectionUI.SetActive(false);
        DontDestroyOnLoad(AugmentSelectionUI);
    }

    void PopulateAugments()
    {
        if (augmentContainer == null) return;
        List<AugmentData> chosenAugments = availableAugments.OrderBy(x => Random.value).Take(3).ToList();
        if (augmentContainer.childCount > 0)
        {
            foreach (Transform child in augmentContainer)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (var augment in chosenAugments)
        {   
            GameObject buttonObj = Instantiate(augmentButtonPrefab, augmentContainer);
            buttonObj.GetComponentInChildren<TMP_Text>().text = augment.augmentName;
            var iconImage = buttonObj.transform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = augment.iconSprite;
            RectTransform rt = iconImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100f * augment.width, 100f * augment.height);
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
        RectTransform rt = iconImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(144f * augment.width, 144f * augment.height);
        Time.timeScale = 1f; 
        AugmentSelectionUI.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (augmentContainer == null || AugmentSelectionUI == null) return;
        PopulateAugments();
        AugmentSelectionUI.SetActive(false);
    }
}
