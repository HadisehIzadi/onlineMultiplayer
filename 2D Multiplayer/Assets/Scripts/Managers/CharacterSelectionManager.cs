using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/*
* Singleton to control the changes on the char sprites and the flow of the scene
*/

public enum ReadyState : byte
{
    notReady,
    ready
}

// Struct for better serialization on the player connection
[Serializable]
public struct PlayerConnectionState
{
    public ReadyState playerState;                  // State of the player
    public PlayerCharSelection playerObject;        // The Object of player selection
    public string playerName;                       // The name of the player when spawn
}

// Struct for better serialization on the container of the character
[Serializable]
public struct CharacterContainer
{
    public Image imageContainer;                    // The image of the character container
    public TextMeshProUGUI nameContainer;           // Character name container
    public GameObject border;                       // The border of the character container when not ready
    public GameObject borderReady;                  // The border of the character container when ready
    public GameObject borderClient;                 // Client border of the character container
    public Image playerIcon;                        // The background icon of the player (p1, p2)
    public GameObject waitingText;                  // The waiting text on the container were no client connected
    public GameObject backgroundShip;               // The background of the ship when not ready
    public Image backgroundShipImage;               // The image of the ship when not ready
    public GameObject backgroundShipReady;          // The background of the ship when ready
    public Image backgroundShipReadyImage;          // The image of the ship when ready
    public GameObject backgroundClientShipReady;    // Client background of the ship when ready
    public Image backgroundClientShipReadyImage;    // Client image of the ship when ready
}

public class CharacterSelectionManager : Singleton<CharacterSelectionManager>
{
    public CharacterDataSO[] charactersData;

    [SerializeField]
    CharacterContainer[] m_charactersContainers;

    [SerializeField]
    GameObject m_readyButton;

    [SerializeField]
    GameObject m_cancelButton;

    [SerializeField]
    float m_timeToStartGame;

    [SerializeField]
    SceneName m_nextScene = SceneName.Gameplay;


    [SerializeField]
    Color m_playerColor;

    [SerializeField]
    PlayerConnectionState m_playerStates;

    [SerializeField]
    GameObject m_playerPrefab;

    [Header("Audio clips")]
    [SerializeField]
    AudioClip m_confirmClip;

    [SerializeField]
    AudioClip m_cancelClip;

    bool m_isTimerOn;
    float m_timer;

    private readonly Color k_selectedColor = new Color32(74, 74, 74, 255);

    void Start()
    {
        m_timer = m_timeToStartGame;
        SpawnPlayerSelection();
    }

    void Update()
    {
        if (m_isTimerOn)
        {
            m_timer -= Time.deltaTime;

            if (m_timer < 0f)
            {
                m_isTimerOn = false;
                StartGame();
            }
        }
    }



    void StartGame()
    {
        LoadingFadeEffect.Instance.FadeAll();
        LoadingSceneManager.Instance.LoadScene(m_nextScene);
    }



    void RemoveSelectedStates()
    {
        for (int i = 0; i < charactersData.Length; i++)
        {
            charactersData[i].isSelected = false;
        }
    }



    void UpdatePlayerState(int stateIndex, ReadyState state)
    {

        m_playerStates.playerState = state;
    }

    void StartGameTimer()
    {
 
        if (m_playerStates.playerState == ReadyState.notReady)
                return;
 
        m_timer = m_timeToStartGame;
        m_isTimerOn = true;
    }



    public bool IsReady(int playerId)
    {
        return charactersData[playerId].isSelected;
    }

    public void SetCharacterColor(int playerId, int characterSelected)
    {
        if (charactersData[characterSelected].isSelected)
        {
            m_charactersContainers[playerId].imageContainer.color = k_selectedColor;
            m_charactersContainers[playerId].nameContainer.color = k_selectedColor;
        }
        else
        {
            m_charactersContainers[playerId].imageContainer.color = Color.white;
            m_charactersContainers[playerId].nameContainer.color = Color.white;
        }
    }

    public void SetCharacterUI(int playerId, int characterSelected)
    {
        m_charactersContainers[playerId].imageContainer.sprite =
            charactersData[characterSelected].characterSprite;

        m_charactersContainers[playerId].backgroundShipImage.sprite =
            charactersData[characterSelected].characterShipSprite;

        m_charactersContainers[playerId].backgroundShipReadyImage.sprite =
            charactersData[characterSelected].characterShipSprite;

        m_charactersContainers[playerId].backgroundClientShipReadyImage.sprite =
            charactersData[characterSelected].characterShipSprite;

        m_charactersContainers[playerId].nameContainer.text =
            charactersData[characterSelected].characterName;

        SetCharacterColor(playerId, characterSelected);
    }

    public void SetPlayebleChar(int playerId, int characterSelected)
    {
        SetCharacterUI(playerId, characterSelected);
        m_charactersContainers[playerId].playerIcon.gameObject.SetActive(true);
        

        m_charactersContainers[playerId].border.SetActive(true);
        m_charactersContainers[playerId].borderReady.SetActive(false);
        m_charactersContainers[playerId].borderClient.SetActive(false);
        m_charactersContainers[playerId].playerIcon.color = m_playerColor;
     

        m_charactersContainers[playerId].backgroundShip.SetActive(true);
        m_charactersContainers[playerId].waitingText.SetActive(false);
    }

    public ReadyState GetReadyState()
    {
            return m_playerStates.playerState;

    }

    public void SpawnPlayerSelection()
    {
        GameObject go =
            Instantiate(
                m_playerPrefab,
                transform.position,
                quaternion.identity);


        m_playerStates.playerObject = go.GetComponent<PlayerCharSelection>();
        m_playerStates.playerName = go.name;


    }




    public void PlayerNotReady(int characterSelected = 0, bool isDisconected = false)
    {
        m_isTimerOn = false;
        m_timer = m_timeToStartGame;


    }



    // Set the player ready if the player is not selected and check if all player are ready to start the countdown
    public void PlayerReady( int playerId, int characterSelected)
    {
        if (!charactersData[characterSelected].isSelected)
        {
            PlayerReadySelect(playerId, characterSelected);
            StartGameTimer();
        }
    }

    // Set the players UI button
    public void SetPlayerReadyUIButtons(bool isReady, int characterSelected)
    {
        if (isReady && !charactersData[characterSelected].isSelected)
        {
            m_readyButton.SetActive(false);
            m_cancelButton.SetActive(true);
        }
        else if (!isReady && charactersData[characterSelected].isSelected)
        {
            m_readyButton.SetActive(true);
            m_cancelButton.SetActive(false);
        }
    }

    // Check if the player has selected the character
    public bool IsSelectedByPlayer(int playerId, int characterSelected)
    {
        return charactersData[characterSelected].playerId == playerId ? true : false;
    }

    void PlayerReadySelect(int playerId, int characterSelected)
    {
        charactersData[characterSelected].isSelected = true;
        charactersData[characterSelected].playerId = playerId;
        m_playerStates.playerState = ReadyState.ready;

        AudioManager.Instance.PlaySoundEffect(m_confirmClip);
    }

    
    void PlayerNotReady(int playerId, int characterSelected)
    {
        charactersData[characterSelected].isSelected = false;
        charactersData[characterSelected].clientId = 0UL;
        charactersData[characterSelected].playerId = -1;


        m_charactersContainers[playerId].border.SetActive(true);
        m_charactersContainers[playerId].borderReady.SetActive(false);
        m_charactersContainers[playerId].borderClient.SetActive(false);
        m_charactersContainers[playerId].backgroundShip.SetActive(true);
        m_charactersContainers[playerId].backgroundShipReady.SetActive(false);
        

        AudioManager.Instance.PlaySoundEffect(m_cancelClip);

    }



}
