using System.Collections;
using UnityEngine;

public class PlayerShipController : MonoBehaviour, IDamagable
{
    public int health;

    [SerializeField]
    private int m_specials = 0;

    [SerializeField]
    int m_maxSpecialPower;

    [SerializeField]
    DefenseMatrix m_defenseShield;

    [SerializeField]
    CharacterDataSO m_characterData;

    [SerializeField]
    GameObject m_explosionVfxPrefab;

    [SerializeField]
    GameObject m_powerupPickupVfxPrefab;

    [SerializeField]
    float m_hitEffectDuration;

    [Header("AudioClips")]
    [SerializeField]
    AudioClip m_hitClip;
    [SerializeField]
    AudioClip m_shieldClip;


    [Header("ShipSprites")]
    [SerializeField]
    SpriteRenderer m_shipRenderer;

    [Header("Runtime set")]
    public PlayerUI playerUI;

    
    public CharacterDataSO characterData;

    public GameplayManager gameplayManager;


    const string k_hitEffect = "_Hit";

    void Update()
    {
            if (!m_defenseShield.isShieldActive &&
                (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.LeftShift)))
            {
                // Tell the server to activate the shield
                ActivateShield();
            } 
    }
    void ActivateShield()
    {
        // Activate the special in case the ship has available
        if (m_specials > 0)
        {
            // Tell the UI to remove the icon
            playerUI.UpdatePowerUp(m_specials, false);

            // Update the UI on clients, reduce the number of specials available
            m_specials--;

        // Activate the special on clients for sync
        m_defenseShield.TurnOnShield();
        AudioManager.Instance?.PlaySoundEffect(m_shieldClip);

        // Update the power up use for the final score
        characterData.powerUpsUsed++;
        }
    }




    void PlayShipHitSound()
    {
            AudioManager.Instance?.PlaySoundEffect(m_hitClip);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {

        // If the collider hit a power-up
        
        if (collider.gameObject.CompareTag("PowerUp"))
        {
            // Check if I have space to take the special
            if (m_specials < m_maxSpecialPower)
            {
                // Update var
                m_specials++;

                // Update UI
                playerUI.UpdatePowerUp(m_specials, true);

                // Show Power-up Pickup VFX
                Instantiate(m_powerupPickupVfxPrefab, transform.position, Quaternion.identity);

                // Remove the power-up
                Destroy(collider.gameObject);
            }
        }
    }


    

    public void Hit(int damage)
    {
        // Update health var
        health -= damage;

        // Update UI
        playerUI.UpdateHealth(health);

        if (health > 0)
        {
            PlayShipHitSound();
        }
        else // (health.Value <= 0)
        {

            Instantiate(m_explosionVfxPrefab,transform.position,Quaternion.identity);

            // Tell the Gameplay manager that I've been defeated
            gameplayManager.PlayerDeath(m_characterData.clientId);
            
        }
    }

    // Set the hit animation effect
    public IEnumerator HitEffect()
    {
        bool active = false;
        float timer = 0f;

        while (timer < m_hitEffectDuration)
        {
            active = !active;
            m_shipRenderer.material.SetInt(k_hitEffect, active ? 1 : 0);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        m_shipRenderer.material.SetInt(k_hitEffect, 0);
    }
}