using System.Collections;
using UnityEngine;

public class PlayerStateMachine
{
    private Player _owner;
    private PlayerBaseState _currState;

    #region STATES
    private PlayerStandState _standState = new PlayerStandState();
    private PlayerCrouchState _crouchState = new PlayerCrouchState();
    private PlayerCoverState _coverState = new PlayerCoverState();
    private PlayerVaultState _jumpState = new PlayerVaultState();
    private PlayerTakedownState _takedownState = new PlayerTakedownState();
    private PlayerDeadState _deadState = new PlayerDeadState();
    #endregion

    public PlayerState PrevState;
    public PlayerState CurrState => _currState.State;

    public float VaultAngle;

    public PlayerStateMachine(Player p)
    {
        _owner = p;

        _currState = _standState;
        _currState.EnterState(_owner);
    }

    public void Update()
    {
        if(_owner.Health.IsDead && _currState.State != PlayerState.DEAD)
        {
            SwitchState(PlayerState.DEAD);
            return;
        }

        _currState.UpdateState();
        _currState.UpdateAnimation();
        _currState.UpdateSwitchState();
    }

    public void FixedUpdate()
    {
        _currState.UpdatePhysics();
    }

    private void SwitchState_INTERNAL(PlayerBaseState newState)
    {
        _currState.ExitState();
        _currState = newState;
        _currState.EnterState(_owner);
    }

    public void SwitchState(PlayerState state)
    {
        if (_currState.State == state)
            return;

        PrevState = _currState.State;

        switch (state)
        {
            case PlayerState.STANDUP:
                SwitchState_INTERNAL(_standState);
                break;
            case PlayerState.CROUCH:
                SwitchState_INTERNAL(_crouchState);
                break;
            case PlayerState.COVER:
                SwitchState_INTERNAL(_coverState);
                break;
            case PlayerState.VAULT:
                SwitchState_INTERNAL(_jumpState);
                break;
            case PlayerState.TAKEDOWN:
                SwitchState_INTERNAL(_takedownState);
                break;
            case PlayerState.DEAD:
                SwitchState_INTERNAL(_deadState);
                break; 
        }
    }
}