using UnityEngine;

public class CircularBullet : MonoBehaviour
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
            GameObject go = Instantiate(
                _smallBulletPrefab,
                firePosition.position,
                firePosition.rotation);

        }
        Destroy(gameObject);    

    }

    private void Start()
    {
        // After a random time amount, blow up and spawn small bullets
        float randomSpawn = Random.Range(1.5f, 3f);
        Invoke(nameof(SpawnBullets), randomSpawn);

    }


}
