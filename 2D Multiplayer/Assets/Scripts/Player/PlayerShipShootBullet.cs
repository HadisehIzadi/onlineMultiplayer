using UnityEngine;
using Unity.Mathematics;

public class PlayerShipShootBullet : MonoBehaviour
{
    [SerializeField]
    int m_fireDamage;

    [SerializeField]
    GameObject m_bulletPrefab;

    [SerializeField]
    Transform m_cannonPosition;

    [SerializeField]
    CharacterDataSO m_characterData;

    [SerializeField]
    GameObject m_shootVfx;

    [SerializeField]
    AudioClip m_shootClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireNewBullet();
        }
    }

    void FireNewBullet()
    {
        SpawnNewBulletVfx();
        GameObject newBullet = GetNewBullet();
        PrepareNewlySpawnedBulltet(newBullet);
        PlayShootBulletSound();
    }

    private void SpawnNewBulletVfx()
    {
        if (m_shootVfx != null)
            Instantiate(m_shootVfx, m_cannonPosition.position, quaternion.identity);

    }

    private GameObject GetNewBullet()
    {
        return Instantiate(
            m_bulletPrefab,
            m_cannonPosition.position,
            quaternion.identity
            );
    }

    private void PrepareNewlySpawnedBulltet(GameObject newBullet)
    {
        BulletController bulletController = newBullet.GetComponent<BulletController>();
        bulletController.damage = m_fireDamage;
        bulletController.characterData = m_characterData;
    }

    void PlayShootBulletSound()
    {
        if (m_shootClip != null)
            AudioManager.Instance?.PlaySoundEffect(m_shootClip);
    }
}
