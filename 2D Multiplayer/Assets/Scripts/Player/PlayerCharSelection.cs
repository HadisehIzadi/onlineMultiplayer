using System.Collections;
using UnityEngine;

public class PlayerCharSelection : MonoBehaviour
{
    [SerializeField]
    private int  m_charSelected;


    [SerializeField]
    private int m_playerId;


    [SerializeField]
    private AudioClip _changedCharacterClip;

    private void Start()
    {

        m_playerId =0 ;
        m_charSelected= 0 ;
             
        CharacterSelectionManager.Instance.SetPlayebleChar(
                m_playerId,
                m_charSelected
                );
       

        // Assign the name of the object base on the player id on every instance
        gameObject.name = $"Player{m_playerId + 1}";
    }

    private void Update()
    {
        if ( CharacterSelectionManager.Instance.GetReadyState() != ReadyState.ready)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ChangeCharacterSelection(-1);
                AudioManager.Instance.PlaySoundEffect(_changedCharacterClip);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ChangeCharacterSelection(1);
                AudioManager.Instance.PlaySoundEffect(_changedCharacterClip);
            }
        }

     
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // Check that the character is not selected
            if (!CharacterSelectionManager.Instance.IsReady(m_charSelected))
            {
                CharacterSelectionManager.Instance.SetPlayerReadyUIButtons(
                    true,
                    m_charSelected);

                Ready();
            }
            else
            {
                // if selected check if is selected by me
                if (CharacterSelectionManager.Instance.IsSelectedByPlayer(
                        m_playerId, m_charSelected))
                {
                    // If it's selected by me, de-select
                    CharacterSelectionManager.Instance.SetPlayerReadyUIButtons(
                        false,
                        m_charSelected);

                    NotReady();
                }
            }
        }
        
    }



    private void ChangeCharacterSelection(int value)
    {
        // Assign a temp value to prevent the call of onchange event in the charSelected
        int charTemp = m_charSelected;
        charTemp += value;
        
        if (charTemp >= CharacterSelectionManager.Instance.charactersData.Length)
            charTemp = 0;
        else if (charTemp < 0)
            charTemp = CharacterSelectionManager.Instance.charactersData.Length - 1;


        m_charSelected = charTemp;
        CharacterSelectionManager.Instance.SetPlayebleChar(
                m_playerId,
                m_charSelected);

    }

    private void Ready()
    {
        CharacterSelectionManager.Instance.PlayerReady(
            m_playerId,
            m_charSelected);
    }

    private void NotReady()
    {
        CharacterSelectionManager.Instance.PlayerNotReady(m_charSelected);
    }

    

    

}