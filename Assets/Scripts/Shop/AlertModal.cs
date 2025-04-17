using UnityEngine;
using TMPro;
using System.Collections;

public class AlertModal : MonoBehaviour
{
    private Transform container;
    private Transform item1;
    private Transform canvas;

    private Coroutine autoCloseCoroutine;

    private void Awake()
    {
        container = transform.Find("Container");
        item1 = container?.Find("Item1");
        canvas = container?.Find("Canvas");

        item1?.gameObject.SetActive(false);
        canvas?.gameObject.SetActive(false);
    }

    public void ShowAlert(string message)
    {
        if (item1 == null || canvas == null) return;

        item1.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);

        RectTransform itemRectTransform = item1.GetComponent<RectTransform>();
        itemRectTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(message);

        // Stop previous coroutine if it's still running
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
        autoCloseCoroutine = StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(2f);
        HideAlert();
    }

    public void HideAlert()
    {
        item1?.gameObject.SetActive(false);
        canvas?.gameObject.SetActive(false);
    }
}
