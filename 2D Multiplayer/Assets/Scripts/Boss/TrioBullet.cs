using UnityEngine;

public class TrioBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject _smallBulletPrefab;

    [SerializeField]
    private Transform[] _firePositions;

    private void SpawnBullets()
    {
        // Spawn the bullets
        foreach (Transform firePosition in _firePositions)
        {
            GameObject newBullet = Instantiate(
                _smallBulletPrefab,
                firePosition.position,
                firePosition.rotation);
            
            
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        SpawnBullets();
    }
}
