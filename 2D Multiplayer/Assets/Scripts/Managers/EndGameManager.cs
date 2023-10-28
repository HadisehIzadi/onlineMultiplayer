using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : Singleton<EndGameManager>
{
    enum EndGameStatus
    {
        victory,
        defeat,
    };

    [SerializeField]
    private EndGameStatus m_status;                     // Set the scene status to now if we are on victory o defeat scene

    [SerializeField]
    private CharacterDataSO[] m_charactersData;         // The characters data use to take some data from there

    [SerializeField]
    private Transform[] m_shipsPositions;               // The final positions of the ships 

    [SerializeField]
    private AudioClip m_endGameClip;                    // The audio clip to reproduce when the scene start

    private int m_shipPositionindex;                    // Var to move every player to different position

    private PlayerShipScore m_bestPlayer;               // Catch who is the best player -> only on server

    private void Start()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySoundEffect(m_endGameClip, 1f);
        SpawnScore();

    }

    public void SpawnScore()
    {

        // We do this only one time when all clients are connected so they sync correctly
        // Tell all clients instance to set the UI base on the server characters data
        int bestScore = -1;
        for (int i = 0; i < m_charactersData.Length; i++)
        {
            if (m_charactersData[i].isSelected)
            {
                GameObject playerScoreResult = Instantiate(
                    m_charactersData[i].spaceshipScorePrefab,
                    m_shipsPositions[m_shipPositionindex].position,
                    Quaternion.identity);

                // Check who has the best score
                // The score is calculated base on the enemies destroyed minus the power-ups the player used
                // Feel free to modify these values
                int enemyDestroyedScore = (m_charactersData[i].enemiesDestroyed * 100);
                int powerUpsUsedScore = (m_charactersData[i].powerUpsUsed * 50);
                int currentFinalScore = enemyDestroyedScore - powerUpsUsedScore;

                var playerShipScore = playerScoreResult.GetComponent<PlayerShipScore>();

                if (currentFinalScore > bestScore)
                {
                    m_bestPlayer = playerShipScore;
                    bestScore = currentFinalScore;
                }
                // Victory or defeat so turn on the appropriate vfx
                bool isVictorious = m_status == EndGameStatus.victory;
                playerShipScore.SetShip(
                    isVictorious,
                    m_charactersData[i].enemiesDestroyed,
                    m_charactersData[i].powerUpsUsed,
                    currentFinalScore);

            }
        }


    }

    // When the button is pressed, start the shutdown process
    public void GoToMenu()
    {

        LoadingSceneManager.Instance.LoadScene(SceneName.Menu, false);
    }

   
    private void SetShipData(
        int enemiesDestroyed,
        int powerUpsUsed,
        int score,
        string spaceShipScoreName)
    {
        // Not optimal, but this is only called one time per ship
        // We use find because we cannot pass a object on RPC
        GameObject spaceShipScore = GameObject.Find(spaceShipScoreName);

        bool isVictorious = m_status == EndGameStatus.victory;
        spaceShipScore.GetComponent<PlayerShipScore>().SetShip(
            isVictorious,
            enemiesDestroyed,
            powerUpsUsed,
            score);
    }

    private void BestShipClientRpc(string spaceShipScoreName)
    {


        // We use find because we cannot pass a object on RPC
        GameObject spaceShipScore = GameObject.Find(spaceShipScoreName);
        spaceShipScore.GetComponent<PlayerShipScore>().BestShip();
    }




}