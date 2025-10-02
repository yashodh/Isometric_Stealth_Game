using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    float _attackFov = 75f;
    private float _meleeDistance = 1.0f;
    private float _rangedDistance = 4.0f;
    private float _currTime = 0.0f;
    private float _attackInterval = 1.5f;

    public override EnemyState State { get { return EnemyState.ATTACK; }  set { } }

    public override void EnterState(Enemy guard)
    {
        Owner = guard;

        Owner.Agent.ResetPath();
        Owner.Agent.speed = 1.0f;

        Owner.EnableFov(2, _rangedDistance, _attackFov);
        Owner.EnableFov(1, _meleeDistance, _attackFov);
    }

    public override void ExitState()
    {
        Owner.Agent.ResetPath();

        Owner.DisableFov(1);
        Owner.DisableFov(2);
    }

    public override void UpdateSwitchState()
    {
        _currTime += Time.deltaTime;
        if (Owner.IsPlayerInFov(_attackFov, _meleeDistance))
        {
            Owner.Agent.ResetPath();

            // do melee
            if (_currTime >= _attackInterval)
            {
                Owner.AnimationControl.SetTrigger(EnemyAnim.Melee);

                // play sfx
                AudioManager.Instance.PlayAudioAt(AudioEnum.Punch, Owner.transform.position);

                Player.Instance.DealDamage(15, HealthContext.Melee);
                Director.Instance.ProcessMessage(Message.PlayerDetected, Owner.GetPosition());
            }
        }
        else if (Owner.IsPlayerInFov(_attackFov, _rangedDistance))
        {
            // Move towards the player
            Owner.Agent.SetDestination(Player.Instance.GetPosition());

            // do shoot
            if(_currTime > _attackInterval)
            {
                Owner.AnimationControl.SetTrigger(EnemyAnim.Shoot);

                // play vfx and sfx
                Owner.PlayShootVfx();
                AudioManager.Instance.PlayAudioAt(AudioEnum.Gunshot, Owner.transform.position);

                Player.Instance.DealDamage(5, HealthContext.Ranged);
                Director.Instance.ProcessMessage(Message.PlayerDetected, Player.Instance.GetPosition());
            }
        }
        else
        {
            // if not within either ranges, change state
            Owner.StateMachine.SwitchState(EnemyState.INVESTIGATE);
        }

        if (_currTime > _attackInterval)
        {
            _currTime = 0.0f;
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
