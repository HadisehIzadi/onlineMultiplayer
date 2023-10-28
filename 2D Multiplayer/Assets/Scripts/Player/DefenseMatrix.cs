using System.Collections;
using UnityEngine;

public class DefenseMatrix : MonoBehaviour, IDamagable
{
    public bool isShieldActive { get; private set; } = false;

    public GameObject shield;
    private CircleCollider2D m_circleCollider2D;

    private void Start()
    {
        m_circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
    }

    public void Hit(int damage)
    {
        TurnOffMatrix();
    }

    public void TurnOnShield()
    {
        isShieldActive = true;

        shield.SetActive(true);
        m_circleCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {


        if (collider.TryGetComponent(out IDamagable damagable))
        {
            damagable.Hit(1);
            TurnOffMatrix();
        }
    }

    private void TurnOffMatrix()
    {
        isShieldActive = false;

        shield.SetActive(false);
        m_circleCollider2D.enabled = false;
    }

    IEnumerator IDamagable.HitEffect()
    {
        return null;
    }
}