using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : EnemyBaseState
{
    float _time = 0.0f;

    #region Properties
    float FOV;
    float alertDist;
    float angleToLook;
    float timeToTurn;
    float dir;
    float turnRate;
    #endregion

    public override EnemyState State { get { return EnemyState.TURN; } set { } }

    public override void EnterState(Enemy guard)
    {
        InitStateData();

        Owner = guard;
        _time = 0.0f;

        Owner.Agent.ResetPath();

        Owner.AnimationControl.SetInt(EnemyAnim.Direction,(int)dir);

        Owner.EnableFov(1, alertDist, FOV);
    }

    public override void ExitState()
    {
        _time = 0.0f;
        Owner.AnimationControl.SetInt(EnemyAnim.Direction,0);

        Owner.DisableFov(1);
    }

    public override void UpdateSwitchState()
    {
        _time += Time.deltaTime;
        if (_time > timeToTurn)
        {
            _time = 0.0f;

            if (Owner.Spawner.PathController != null)
            {
                Owner.StateMachine.SwitchState(EnemyState.PATROL);
                return;
            }
            else
            {
                Owner.StateMachine.SwitchState(EnemyState.IDLE);
                return;
            }
        }
        else
        {
            Owner.transform.Rotate(Vector3.up * turnRate * Time.deltaTime);

            if (Owner.IsPlayerInFov(FOV, alertDist))
            {
                Owner.StateMachine.SwitchState(EnemyState.ATTACK);
                return;
            }
        }
    }

    public override void UpdateAnimation()
    {
        EnemyAnim anim = EnemyAnim.Move;
        float move = Owner.AnimationControl.GetFloat(anim);

        if (move > 0)
            Owner.AnimationControl.SetFloat(anim, move- Time.deltaTime * 1.0f);
    }

    private void InitStateData()
    {
        FOV = Game.Instance.GuardData.LookAround_FOV;
        alertDist = Game.Instance.GuardData.LookAround_Dist;

        angleToLook = Game.Instance.GuardData.LookAround_TurnAngle;
        timeToTurn = Game.Instance.GuardData.LookAround_TurnTime;

        dir = Random.value;
        if (dir < 0.5)
            dir = -1;
        else
            dir = 1;

        turnRate = dir * angleToLook / timeToTurn;
    }
}
