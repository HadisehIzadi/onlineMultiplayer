using UnityEngine;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Dependencies.NCalc;
using TMPro;

public class BaseEnemyBehavior : MonoBehaviour, IDamagable
{
    protected enum EnemyMovementType
    {
        linear,
        sineWave,

        // you can add more movement types here

        COUNT //MAX - used to get random value
    }

    protected enum EnemyState : byte
    {
        active,
        defeatAnimation,
        defeated
    }

    [SerializeField]
    protected float m_EnemySpeed = 4f;


    [SerializeField]
    protected bool m_UsesEnemyLifetime = true;



    [SerializeField]
    public int m_EnemyHealthPoints = 3;

    [SerializeField]
    protected GameObject m_VfxExplosion;


    protected EnemyState m_EnemyState = EnemyState.active;

    protected EnemyMovementType m_EnemyMovementType;

    protected Vector2 m_Direction = Vector2.left;

    protected float m_WaveAmplitude;

    [SerializeField]
    private SpriteRenderer m_sprite;

    [SerializeField]
    private float m_hitEffectDuration;


    private void Start()
    {
        m_WaveAmplitude = Random.Range(2f, 6f);

        m_EnemyMovementType = GetRandomEnemyMovementType();
    }


    protected virtual void Update()
    {
        if (m_EnemyState/*.Value*/ == EnemyState.active)
        {
            UpdateActive();
        }
        else if (m_EnemyState/*.Value*/ == EnemyState.defeatAnimation)
        {
            UpdateDefeatedAnimation();
        }
        else // (m_EnemyState.Value == EnemyState.defeated)
        {
            DespawnEnemy();
        }
        if (m_EnemyHealthPoints <= 0)
        {
            m_EnemyState/*.Value*/ = EnemyState.defeatAnimation;
            DespawnEnemy();

        }

    }

    protected virtual void UpdateActive()
    {
    }

    protected virtual void UpdateDefeatedAnimation()
    {
    }

    protected virtual void MoveEnemy()
    {
        if (m_EnemyMovementType == EnemyMovementType.sineWave)
        {
            m_Direction.x = -1f; //to move from right to left
            m_Direction.y = Mathf.Sin(Time.time * m_WaveAmplitude);

            m_Direction.Normalize();
        }

        // move the enemy in the desired direction
        transform.Translate(m_Direction * m_EnemySpeed * Time.deltaTime);
    }

    protected EnemyMovementType GetRandomEnemyMovementType()
    {
        int randomValue = Random.Range(0, (int)EnemyMovementType.COUNT);

        return (EnemyMovementType)randomValue;
    }

    protected void DespawnEnemy()
    {
        gameObject.SetActive(false);
        if (m_VfxExplosion != null)
            Instantiate(m_VfxExplosion, transform.position, Quaternion.identity);
        PowerUpSpawnController.instance.OnPowerUpSpawn(transform.position);
        Destroy(gameObject);
    }


    public virtual void Hit(int damage)
    {

        m_EnemyHealthPoints -= 1;

        StopCoroutine(HitEffect());
        StartCoroutine(HitEffect());
    }

    public IEnumerator HitEffect()
    {
        bool active = false;
        float timer = 0f;

        while (timer < m_hitEffectDuration)
        {
            active = !active;
            m_sprite.material.SetFloat("_Hit", active ? 1 : 0);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        m_sprite.material.SetInt("_Hit", 0);
    }
}
