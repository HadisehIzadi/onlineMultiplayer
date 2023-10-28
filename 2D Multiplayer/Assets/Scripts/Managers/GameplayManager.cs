using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public static Action<ulong> OnPlayerDefeated;

    [SerializeField]
    private CharacterDataSO[] m_charactersData;

    [SerializeField]
    private PlayerUI[] m_playersUI;

    [SerializeField]
    private GameObject m_deathUI;

    [SerializeField]
    private Transform m_shipStartingPosition;

    private int m_numberOfPlayerConnected;
    private List<ulong> m_connectedClients = new List<ulong>();
    private List<PlayerShipController> m_playerShips = new List<PlayerShipController>();


    
    private void Start()
    {
        int i = GetShipIndex();
        GameObject playerGO = Instantiate(m_charactersData[i].spaceshipPrefab, m_shipStartingPosition.position, Quaternion.identity)as GameObject;
        PlayerShipController playerShipController =
            playerGO.GetComponent<PlayerShipController>();
        playerGO.name = m_charactersData[i].characterName;
        m_charactersData[i].playerId = 0; 
        playerShipController.characterData = m_charactersData[i];
        playerShipController.gameplayManager = this;

        m_playerShips.Add(playerShipController);
        SetPlayerUI(i, m_charactersData[i].characterName);


    }

    public int GetShipIndex()
    {
        int currentShipIndex = 0;
        for (int i = 0; i < m_charactersData.Length; i++)
        {
            if (m_charactersData[i].isSelected)
                currentShipIndex= i;
        }


        return currentShipIndex;

    }
    public void PlayerDeath(ulong clientId)
    {
            LoadingSceneManager.Instance.LoadScene(SceneName.Defeat);

    }





    //[ClientRpc]
    private void Load/*ClientRpc*/()
    {
       // if (IsServer)
        //    return;

        LoadingFadeEffect.Instance.FadeAll();
    }

    //[ClientRpc]
    private void SetPlayerUI(int charIndex, string playerShipName)
    {
        // Not optimal, but this is only called one time per ship
        // We do this because we can not pass a GameObject in an RPC
        GameObject playerSpaceship = GameObject.Find(playerShipName);

        PlayerShipController playerShipController =
            playerSpaceship.GetComponent<PlayerShipController>();
        m_playersUI[m_charactersData[charIndex].playerId].SetUI(
            m_charactersData[charIndex].playerId,
            m_charactersData[charIndex].iconSprite,
            playerShipController.health,
            m_charactersData[charIndex].darkColor);

        // Pass the UI to the player
        playerShipController.playerUI = m_playersUI[m_charactersData[charIndex].playerId];
    }


    public void BossDefeat()
    {
        LoadingSceneManager.Instance.LoadScene(SceneName.Victory);
    }

    public void ExitToMenu()
    {
        LoadingSceneManager.Instance.LoadScene(SceneName.Gameplay);
    }

}