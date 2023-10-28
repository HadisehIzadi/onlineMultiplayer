using UnityEngine;

public class SpaceShooterEnemyBehavior : BaseEnemyBehavior
{
    [SerializeField]
    public GameObject m_EnemyBulletPrefab;

    [SerializeField]
    private float m_ShootingCooldown = 2f;


    [SerializeField]
    private AudioClip m_shootClip;

    private float m_CurrentCooldownTime = 0f;


    protected override void UpdateActive()
    {
        MoveEnemy();

        m_CurrentCooldownTime += Time.deltaTime;
        if (m_CurrentCooldownTime >= m_ShootingCooldown)
        {
            m_CurrentCooldownTime = 0f;
            ShootLaser();
        }
    }

    protected override void UpdateDefeatedAnimation()
    {
        m_EnemyState = EnemyState.defeated;
    }

    private void ShootLaser()
    {

        var newEnemyLaser = Instantiate(m_EnemyBulletPrefab,transform.position, Quaternion.identity);

        PlayShootAudio();

        var bulletController = newEnemyLaser.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.m_Owner = gameObject;
        }

        newEnemyLaser.transform.position = this.gameObject.transform.position;
    }

    private void PlayShootAudio()
    {
        if(m_shootClip!=null)
            AudioManager.Instance.PlaySoundEffect(m_shootClip);
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        // check if it's collided with a player spaceship
        var spacheshipController = otherObject.gameObject.GetComponent<PlayerShipController>();
        if (spacheshipController != null)
        {
            // tell the spaceship that it's taken damage
            spacheshipController.Hit(1);

            // enemy explodes when it collides with the a player's ship
            m_EnemyState = EnemyState.defeatAnimation;
        }

        // check if it's collided with a player's bullet
        var shipBulletBehavior = otherObject.gameObject.GetComponent<BulletController>();
        if (shipBulletBehavior != null && shipBulletBehavior.m_Owner != this.gameObject)
        {
            // if so, take one health point away from enemy
            m_EnemyHealthPoints -= 1;
        }
    }


}