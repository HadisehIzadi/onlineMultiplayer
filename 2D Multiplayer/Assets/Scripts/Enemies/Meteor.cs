using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int m_damage = 1;
    
    [SerializeField]
    private int m_health = 1;

    [SerializeField]
    private GameObject m_vfxExplosion;

    [SerializeField]
    private Sprite[] m_meteors;

    [SerializeField]
    private GameObject m_meteorSprite;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    float m_hitEffectDuration = 0.2f;

    [Header("Range for random scale value")]
    [SerializeField]
    private float m_scaleMin = 0.8f;

    [SerializeField]
    private float m_scaleMax = 1.5f;


    public Vector3 direction = Vector3.right;

    public float speed = 2f;

    private void Update()
    {

        transform.Translate(speed * Time.deltaTime * direction);
    }
    private void Start()
    {
        // Randomly select the sprite to use 
        m_spriteRenderer.GetComponent<SpriteRenderer>().sprite =
            m_meteors[Random.Range(0, m_meteors.Length)];

        // Randomly scale the meteor
        float randomScale = Random.Range(m_scaleMin, m_scaleMax);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }



    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IDamagable damagable))
        {
            // Hit the object that collide with me
            damagable.Hit(m_damage);

            // Hit me too!
            Hit(m_damage);
        }
    }

    public IEnumerator HitEffect()
    {
        bool active = false;
        float timer = 0f;
        while (timer < m_hitEffectDuration)
        {
            active = !active;
            m_spriteRenderer.material.SetInt("_Hit", active ? 1 : 0);

            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }

        m_spriteRenderer.material.SetInt("_Hit", 0);
    }

    public void Hit(int damage)
    {
        m_health -= damage;

        if (m_health <= 0)
        {
            if (m_vfxExplosion!= null)
                Instantiate(m_vfxExplosion,transform.position,Quaternion.identity);

           Destroy(gameObject);
        }

        StopCoroutine(HitEffect());
        StartCoroutine(HitEffect());
    }
}