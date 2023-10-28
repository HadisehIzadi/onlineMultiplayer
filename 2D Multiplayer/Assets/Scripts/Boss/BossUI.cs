using UnityEngine;
using UnityEngine.UI;

/*
    Script that controls the boss health ui
    set by boss controller when spawn
*/

public class BossUI : MonoBehaviour
{
    [SerializeField]
    Slider m_healthSlider;

    [SerializeField]
    Image m_healthImage;

    [SerializeField]
    HealthColor m_healthColor;

    int maxHealth;
   
  
    private void UpdateUI(float currentHealth)
    {

        m_healthSlider.value = currentHealth;
        m_healthImage.color = m_healthColor.GetHealthColor(currentHealth);
    }

    public void SetHealth(int health)
    {
        
        maxHealth = health;
        m_healthImage.color = m_healthColor.normalColor;
        gameObject.SetActive(true);
    }

    public void UpdateUI(int currentHealth)
    {

        float convertedHealth = (float)currentHealth / maxHealth;
        m_healthSlider.value = convertedHealth;
        m_healthImage.color = m_healthColor.GetHealthColor(convertedHealth);

        UpdateUI(convertedHealth);
    }



    private void Start()
    {
        gameObject.SetActive(false);

    }
}
