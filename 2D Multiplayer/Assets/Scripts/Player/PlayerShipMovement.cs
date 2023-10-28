using System;
using UnityEngine;

public class PlayerShipMovement : MonoBehaviour
{


    [Serializable]
    public struct PlayerLimits
    {
        public float minLimit;
        public float maxLimit;
    }



    [SerializeField]
    PlayerLimits m_verticalLimits;

    [SerializeField]
    PlayerLimits m_hortizontalLimits;



    [SerializeField]
    private float m_speed;

    private float m_inputX;
    private float m_inputY;


    const string k_horizontalAxis = "Horizontal";
    const string k_verticalAxis = "Vertical";

    // Update is called once per frame
    void Update()
    {
        HandleMoveTypeMomentum();
        AdjustInputValuesBasedOnPositionLimits();
        MovePlayerShip();
    }



    private void HandleMoveTypeConstant()
    {
        m_inputX = 0f;
        m_inputY = 0f;

        // Horizontal input
        if (Input.GetKey(KeyCode.D))
        {
            m_inputX = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_inputX = -1f;
        }

        // Vertical input and set the ship sprite
        if (Input.GetKey(KeyCode.W))
        {
            m_inputY = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_inputY = -1f;
        }
    }

    private void HandleMoveTypeMomentum()
    {
        m_inputX = Input.GetAxis(k_horizontalAxis);
        m_inputY = Input.GetAxis(k_verticalAxis);
    }


    // Check the limits of the player and adjust the input
    private void AdjustInputValuesBasedOnPositionLimits()
    {
        PlayerMovementInputLimitAdjuster.AdjustInputValuesBasedOnPositionLimits(
            transform.position,
            ref m_inputX,
            ref m_inputY,
            m_hortizontalLimits,
            m_verticalLimits
        );
    }

    private void MovePlayerShip()
    {
        // Take the value from the input and multiply by speed and time
        float speedTimesDeltaTime = m_speed * Time.deltaTime;

        float newYposition = m_inputY * speedTimesDeltaTime;
        float newXposition = m_inputX * speedTimesDeltaTime;

        // move the ship
        transform.Translate(newXposition, newYposition, 0f);
    }
}