using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour

{
    public GameObject spaceGhostEnemyPrefabToSpawn;
    public GameObject spaceShooterEnemyPrefabToSpawn;


    [Header("Boss")]
    [SerializeField]
    BossUI m_bossUI;

    [SerializeField]
    private GameObject m_bossPrefabToSpawn;

    [SerializeField]
    private Transform m_bossPosition;

    [Header("Enemies")]
    [SerializeField]
    private float m_EnemySpawnTime = 1.8f;


    [SerializeField]
    private float m_bossSpawnTime = 75f;
        

    [Header("Meteors")]
    [SerializeField]
    private GameObject m_meteorPrefab;

    [SerializeField]
    private float m_meteorSpawningTime;

    [Space]
    [SerializeField]
    AudioClip m_warningClip;

    [SerializeField]
    GameObject m_warningUI;

    private Vector3 m_CurrentNewEnemyPosition = new Vector3();
    private float m_CurrentEnemySpawnTime = 0f;
    private Vector3 m_CurrentNewMeteorPosition = new Vector3();
    private float m_CurrentMeteorSpawnTime = 0f;
    private float m_CurrentBossSpawnTime = 0f;
    private bool m_IsSpawning = true;

    private void Start()
    {
        // Initialize the enemy and meteor spawn position based on my owning GO's x position
        m_CurrentNewEnemyPosition.x = transform.position.x;
        m_CurrentNewEnemyPosition.z = 0f;

        m_CurrentNewMeteorPosition.x = transform.position.x;
        m_CurrentNewMeteorPosition.z = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsSpawning)
            return;

        UpdateEnemySpawning();

        UpdateMeteorSpawning();

        UpdateBossSpawning();
    }

    private void UpdateEnemySpawning()
    {
        m_CurrentEnemySpawnTime += Time.deltaTime;
        if (m_CurrentEnemySpawnTime >= m_EnemySpawnTime)
        {
            // update the new enemy's spawn position(y value). This way we don't have to allocate
            // a new Vector3 each time.
            m_CurrentNewEnemyPosition.y = Random.Range(-5f, 5f);

            var nextPrefabToSpawn = GetNextRandomEnemyPrefabToSpawn();


            Instantiate(nextPrefabToSpawn, m_CurrentNewEnemyPosition, Quaternion.identity); 

            m_CurrentEnemySpawnTime = 0f;
        }
    }

    GameObject GetNextRandomEnemyPrefabToSpawn()
    {
        int randomPick = Random.Range(0, 99);

        if (randomPick < 50)
        {
            return spaceGhostEnemyPrefabToSpawn;
        }

        // randomPick >= 50
        return spaceShooterEnemyPrefabToSpawn;
    }

    private void UpdateMeteorSpawning()
    {
        m_CurrentMeteorSpawnTime += Time.deltaTime;
        if (m_CurrentMeteorSpawnTime > m_meteorSpawningTime)
        {
            SpawnNewMeteor();

            m_CurrentMeteorSpawnTime = 0f;
        }
    }

    void SpawnNewMeteor()
    {
        // The min and max Y pos for spawning the meteors
        m_CurrentNewMeteorPosition.y = Random.Range(-5f, 6f);
        Instantiate(m_meteorPrefab, m_CurrentNewMeteorPosition,Quaternion.identity);
    }

    private void UpdateBossSpawning()
    {
        m_CurrentBossSpawnTime += Time.deltaTime;
        if (m_CurrentBossSpawnTime >= m_bossSpawnTime)
        {
            m_IsSpawning = false;
            StartCoroutine(BossAppear());
        }
    }
    
    IEnumerator BossAppear()
    {
        AudioManager.Instance.SwitchMusic(AudioManager.Instance.bossMusic);

        // Warning title and sound
        PlayWarnnig();
        m_warningUI.SetActive(true);
        AudioManager.Instance.PlaySoundEffect(m_warningClip);

        // Same time as audio length
        yield return new WaitForSeconds(m_warningClip.length);

        StopWarnnig();
        m_warningUI.SetActive(false);

        GameObject boss = Instantiate(
        m_bossPrefabToSpawn,
        transform.position, Quaternion.identity);

        BossController bossController = boss.GetComponent<BossController>();
        bossController.StartBoss(m_bossPosition.position);
        bossController.SetUI(m_bossUI);
        boss.name = "BOSS";
    }


    void PlayWarnnig()
    {

        m_warningUI.SetActive(true);

        AudioManager.Instance.PlaySoundEffect(m_warningClip);
    }


    void StopWarnnig()
    {
        m_warningUI.SetActive(false);
    }
}