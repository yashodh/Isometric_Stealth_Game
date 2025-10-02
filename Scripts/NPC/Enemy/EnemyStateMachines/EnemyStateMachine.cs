using System.Collections;
using UnityEngine;


public class EnemyStateMachine
{
    private Enemy _owner;
    private EnemyBaseState _currState;

    #region STATES
    private EnemyIdleState _idleState = new EnemyIdleState();
    private EnemyPatrolState _patrolState = new EnemyPatrolState();
    private EnemyAttackState _attackState = new EnemyAttackState();
    private EnemyInvestigateState _investigateState = new EnemyInvestigateState();
    private EnemyTurnState _turnState = new EnemyTurnState();
    private EnemyHideState _hideState = new EnemyHideState();
    private EnemyDeadState _deadState = new EnemyDeadState();
    #endregion

    public EnemyState PrevState;
    public EnemyState CurrState => _currState.State;

    public EnemyStateMachine(Enemy g)
    {
        _owner = g;

        _currState = _idleState;
        _currState.EnterState(_owner);
    }

    public void Update()
    {
        // dead state is common
        if (_owner.Health.IsDead && CurrState != EnemyState.DEAD)
        {
            SwitchState(EnemyState.DEAD);
            return;
        }

        if(Player.Instance.Health.IsDead && CurrState == EnemyState.ATTACK)
        {
            SwitchState(EnemyState.TURN);
            return;
        }

        _currState.UpdateAnimation();
        _currState.UpdateSwitchState();
    }

    private void SwitchState_INTERNAL(EnemyBaseState newState)
    {
        _currState.ExitState();
        _currState = newState;
        _currState.EnterState(_owner);
    }

    public void SwitchState(EnemyState state)
    {
        if (_currState.State == state)
            return;

        switch (state)
        {
            case EnemyState.IDLE:
                SwitchState_INTERNAL(_idleState);
                break;
            case EnemyState.PATROL:
                SwitchState_INTERNAL(_patrolState);
                break;
            case EnemyState.INVESTIGATE:
                SwitchState_INTERNAL(_investigateState);
                break;
            case EnemyState.ATTACK:
                SwitchState_INTERNAL(_attackState);
                break;
            case EnemyState.TURN:
                SwitchState_INTERNAL(_turnState);
                break;
            case EnemyState.HIDE:
                SwitchState_INTERNAL(_hideState);
                break;
            case EnemyState.DEAD:
                SwitchState_INTERNAL(_deadState);
                break;
        }
    }
}
