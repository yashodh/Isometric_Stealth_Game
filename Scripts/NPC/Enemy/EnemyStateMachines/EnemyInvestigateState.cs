using System.Collections.Generic;
using UnityEngine;

public class EnemyInvestigateState : EnemyBaseState
{
    private float _currTime = 0f;
    private float _stateTime = 4.0f;

    private float _attackFov = 75f;
    private float _attackDistance = 4.0f;

    private float _investigateFov = 135f;
    private float _investigateDistance = 5.0f;

    public override EnemyState State { get { return EnemyState.INVESTIGATE; } set { } }

    public override void EnterState(Enemy guard)
    {
        Owner = guard;
        
        Owner.Agent.ResetPath();
        Owner.Agent.speed = 1.0f;

        Owner.EnableFov(2, _investigateDistance, _investigateFov);
        Owner.EnableFov(1, _attackDistance, _attackFov);
    }

    public override void ExitState()
    {
        Owner.DisableFov(1);
        Owner.DisableFov(2);

        Owner.Agent.ResetPath();
    }

    public override void UpdateSwitchState()
    {
        _currTime += Time.deltaTime;

        if (Owner.IsPlayerInFov(_attackFov, _attackDistance))
        {
            // if within attack state fov. switch to attack state
            _currTime = 0.0f;
            Owner.StateMachine.SwitchState(EnemyState.ATTACK);
            return;
        }
        else if (Owner.IsPlayerInFov(_investigateFov, _investigateDistance))
        {
            // move towards the player position
            _currTime = 0.0f;
            Owner.LocationToInvestigate = Player.Instance.GetPosition();
            Owner.Agent.SetDestination(Owner.LocationToInvestigate);
        }
        else if(_currTime > _stateTime)
        {
            // wait before switching to Turn state
            _currTime = 0.0f;
            Owner.StateMachine.SwitchState(EnemyState.TURN);
        }
        else
        {
            Owner.Agent.SetDestination(Owner.LocationToInvestigate);
        }
    }

    public override void UpdateAnimation()
    {
        EnemyAnim anim = EnemyAnim.Move;
        float move = Owner.AnimationControl.GetFloat(anim);

        if (Owner.Agent.hasPath)
        {
            if (move < 1.0f)
                Owner.AnimationControl.SetFloat(anim, move + Time.deltaTime * 1.0f);
        }
        else
        {
            if (move > 0)
                Owner.AnimationControl.SetFloat(anim, move - Time.deltaTime * 1.0f);
        }
    }
}
