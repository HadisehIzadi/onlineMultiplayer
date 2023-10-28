using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public Vector3 direction = Vector3.right;

    public float speed = 2f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }
    private enum BulletOwner
    {
        enemy,
        player
    };

    public int damage = 1;

    [HideInInspector]
    public CharacterDataSO characterData;
    
    [SerializeField]
    private BulletOwner m_owner;

    [HideInInspector]
    public GameObject m_Owner { get; set; } = null;

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.TryGetComponent(out IDamagable damagable))
        {

            if (m_owner == BulletOwner.player)
            {
                // For the final score
                characterData.enemiesDestroyed++;
            }
            damagable.Hit(damage);
            Destroy(gameObject);
        }
    }

}
