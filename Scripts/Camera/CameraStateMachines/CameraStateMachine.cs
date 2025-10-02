using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine
{
    private CameraController _camCont;
    private CameraBaseState _currState;

    #region STATES
    private CameraTPState _tpState = new CameraTPState();
    private CameraIsometricState _isometricState = new CameraIsometricState();
    private CameraTakedownState _takedownState = new CameraTakedownState();
    #endregion

    public CameraState PrevState;
    public CameraState CurrState => _currState.State;

    public CameraStateMachine(CameraController c)
    {
        _camCont = c;

        _currState = _isometricState;
        _currState.EnterState(_camCont);
    }

    public void Update()
    {
        _currState.UpdateState();
    }

    private void SwitchState_INTERNAL(CameraBaseState state)
    {
        _currState.ExitState();
        _currState = state;
        _currState.EnterState(_camCont);
    }

    public void SwitchState(CameraState state)
    {
        if (_currState.State == state)
            return;

        PrevState = _currState.State;

        switch (state)
        {
            case CameraState.ThirdPerson:
                SwitchState_INTERNAL(_tpState);
                break;
            case CameraState.Isometric:
                SwitchState_INTERNAL(_isometricState);
                break;
            case CameraState.Takedown:
                SwitchState_INTERNAL(_takedownState);
                break;
        }
    }
}
