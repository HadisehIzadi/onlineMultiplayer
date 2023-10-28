using UnityEngine;

/*
    Script that controls how the boss is going to work,
    the different behaviours are set on different scripts. 
    Here you can add new states
*/

public class BossController : MonoBehaviour
{
    [SerializeField]
    private int m_damage;

    [Header("States for the boss")]
    [SerializeField]
    private BossEnterState m_enterState;

    [SerializeField]
    private BaseBossState m_fireState;

    [SerializeField]
    private BaseBossState m_misileBarrageState;

    [SerializeField]
    private BaseBossState m_idleState;

    [SerializeField]
    private BaseBossState m_deathState;


    private BossUI bossUI;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // When the players get close to me do some damage
        if (collider.TryGetComponent(out PlayerShipController playerShip))
        {
            playerShip.Hit(m_damage);
        }
    }    

    // When hit update the UI
    public void OnHit(int currentHealth)
    {
        bossUI.UpdateUI(currentHealth);
    }

    // This will set the starting state for the boss -> enter state
    public void StartBoss(Vector3 initialPositionForEnterState)
    {
        m_enterState.initialPosition = initialPositionForEnterState;
        SetState(BossState.enter);
    }

    // Set the boss state to run
    // You can add more states to the boss
    //..
    public void SetState(BossState state)
    {

        switch (state)
        {
            case BossState.enter:
                m_enterState.RunState();
                break;
            case BossState.fire:
                m_fireState.RunState();
                break;
            case BossState.misileBarrage:
                m_misileBarrageState.RunState();
                break;
            case BossState.idle:
                m_idleState.RunState();
                break;
            case BossState.death:
                // Stop all coroutines from other state
                // because the death can override any state
                m_enterState.StopState();
                m_fireState.StopState();
                m_misileBarrageState.StopState();
                m_idleState.StopState();

                m_deathState.RunState();
                break;
        }
    }

    // Set the boss UI
    public void SetUI(BossUI bossUI)
    {

        BossHealth bossHealth = GetComponentInChildren<BossHealth>();
        this.bossUI = bossUI;
        bossUI.SetHealth(bossHealth.m_health);
    }


}
