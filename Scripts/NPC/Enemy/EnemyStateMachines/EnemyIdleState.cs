using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    float angle;
    float distSqr;
    private float _time = 0.0f;
    float WaitTime;

    #region Properties
    float FOV;
    float alertDist;
    float alertDistSqr;
    #endregion

    public override EnemyState State { get { return EnemyState.IDLE; } set { } }

    public override void EnterState(Enemy guard)
    {
        InitStateData();

        _time = 0.0f;

        Owner = guard;
        Owner.Agent.ResetPath();

        Owner.EnableFov(1, alertDist, FOV);
    }

    public override void ExitState()
    {
        Owner.DisableFov(1);
        _time = 0.0f;
    }

    public override void UpdateSwitchState()
    {
        angle = Vector3.Angle(Player.Instance.GetPosition() - Owner.GetPosition(), Owner.GetForward());
        distSqr = Vector3.SqrMagnitude(Player.Instance.GetPosition() - Owner.GetPosition());

        _time += Time.deltaTime;

        if (_time > WaitTime)
        {
            _time = 0.0f;

            if (Owner.Spawner.PathController != null)
                Owner.StateMachine.SwitchState(EnemyState.PATROL);
            else
                Owner.StateMachine.SwitchState(EnemyState.TURN);

            return;
        }
        else if (angle <= FOV / 2 && angle >= -FOV / 2 && distSqr < alertDistSqr)
        {
            Owner.StateMachine.SwitchState(EnemyState.ATTACK);
            return;
        }
    }

    public override void UpdateAnimation()
    {
        EnemyAnim anim = EnemyAnim.Move;
        float move = Owner.AnimationControl.GetFloat(anim);

        if (move > 0)
            Owner.AnimationControl.SetFloat(anim, move - Time.deltaTime * 1.0f);
    }

    private void InitStateData()
    {
        FOV = Game.Instance.GuardData.Idle_FOV;
        alertDist = Game.Instance.GuardData.Idle_Dist;
        WaitTime = Game.Instance.GuardData.Idle_StateTime;
        alertDistSqr = Mathf.Pow(alertDist, 2);
    }
}
