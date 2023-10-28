using UnityEngine;

public class SmallBullet : MonoBehaviour
{
    private int m_damage = 1;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IDamagable damagable))
        {
            damagable.Hit(m_damage);

            Destroy(gameObject);
        }
    }
}
