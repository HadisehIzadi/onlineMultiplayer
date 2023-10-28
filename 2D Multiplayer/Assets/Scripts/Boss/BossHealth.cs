using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamagable
{

    [Header("Boss health minimum 1")]
    [Min(1)]
    public int  m_health;

    [SerializeField]
    private SpriteRenderer[] m_sprites;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_hitEffectDuration;



    [SerializeField]
    private BossController m_bossController;

    private bool m_isInmmune;

    private const string k_effectHit = "_Hit";
    private const string k_animHit = "hit";


    // For when someone hits me
    public void Hit(int damage)
    {
        if (m_isInmmune)
           return;

        m_health -= damage;
        m_bossController.OnHit(m_health);

        HitEffectCoroutine();

        if (m_health <= 0)
        {
            // If health is below or equal to 0 change to death state
            m_bossController.SetState(BossState.death);
        }
    }

    
    private void HitEffectCoroutine()
    {
        StopCoroutine(HitEffect());
        StartCoroutine(HitEffect());
    }

    // The hit effect use in the game
    public IEnumerator HitEffect()
    {
        m_isInmmune = true;
        bool active = false;
        float timer = 0f;

        while (timer < m_hitEffectDuration)
        {
            active = !active;
            foreach (var sprite in m_sprites)
            {
                sprite.material.SetInt(k_effectHit, active ? 1 : 0);
            }
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        foreach (var sprite in m_sprites)
        {
            sprite.material.SetInt(k_effectHit, 0);
        }


        yield return new WaitForSeconds(0.2f);
        m_isInmmune = false;
    }
}
