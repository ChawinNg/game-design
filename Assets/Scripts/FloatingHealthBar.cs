using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void updateHealthBar(float health, float maxHealth)
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        slider.value = health / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
