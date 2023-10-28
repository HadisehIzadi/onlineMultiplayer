using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

/*
    This script set and update the values of the player ship UI
    set: by game manager
*/

public class PlayerUI : MonoBehaviour
{
    // Struct for a better organization of the health UI 
    [Serializable]
    public struct HealthUI
    {
        public GameObject healthUI;
        public Image playerIconImage;
        public TextMeshProUGUI playerIdText;
        public Slider healthSlider;
        public Image healthImage;
        public HealthColor healthColor;
        public GameObject[] powerUp;
    }


    [SerializeField]
    HealthUI m_healthUI;                // A struct for all the data relate to the health UI

    [SerializeField]
    [Header("Set in runtime")]
    public int maxHealth;               // Max health the player has, use for the conversion to the
                                        // slider and the coloring of the bar
    
    //[ClientRpc]
    void UpdateHealth(float currentHealth)
    {
       // if (IsServer)
          //  return;
        
        m_healthUI.healthSlider.value = currentHealth;
        m_healthUI.healthImage.color = m_healthUI.healthColor.GetHealthColor(currentHealth);

        if (currentHealth <= 0f)
        {
            // Turn off lifeUI
            m_healthUI.healthUI.SetActive(false);

        }
    }
     
    // TODO: check if the initial values are set on client
    // Set the initial values of the UI
    public void SetUI(
        int playerId,
        Sprite playerIcon,
        int maxHealth,
        Color color)
    {
        m_healthUI.playerIconImage.sprite = playerIcon;
        m_healthUI.playerIdText.color = color;
        m_healthUI.playerIdText.text = $"P{(playerId + 1)}";


        this.maxHealth = maxHealth;
        m_healthUI.healthImage.color = m_healthUI.healthColor.normalColor;

        // Turn on my lifeUI
        m_healthUI.healthUI.SetActive(true);

    }

    // Update the UI health 
    public void UpdateHealth(int currentHealth)
    {
        // Don't let health to go below 
        currentHealth = currentHealth < 0 ? 0 : currentHealth;

        float convertedHealth = (float)currentHealth / (float)maxHealth;
        m_healthUI.healthSlider.value = convertedHealth;
        m_healthUI.healthImage.color = m_healthUI.healthColor.GetHealthColor(convertedHealth);

        if (currentHealth <= 0)
        {
            // Turn off lifeUI
            m_healthUI.healthUI.SetActive(false);

        }

        UpdateHealth(convertedHealth);
    }

    // Activate/deactivate the power up icons base on the index pass
    public void UpdatePowerUp(int index, bool hasSpecial)
    {
        m_healthUI.powerUp[index - 1].SetActive(hasSpecial);
        UpdatePowerUpClientRpc(index, hasSpecial);
    }

    void UpdatePowerUpClientRpc(int index, bool hasSpecial)
    {
        m_healthUI.powerUp[index - 1].SetActive(hasSpecial);
    }
}