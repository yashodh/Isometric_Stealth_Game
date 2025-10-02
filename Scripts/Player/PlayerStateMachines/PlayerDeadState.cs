using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    float _time;

    float UpTime = 5.0f;

    public override PlayerState State { get { return PlayerState.DEAD; } set { } }

    public override void EnterState(Player player)
    {
        Owner = player;
        Owner.AnimationControl.SetTrigger(PlayerAnim.DeathFallback);

        Owner.AudioControl.Stop();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        _time += Time.deltaTime;

        if (_time > UpTime)
        {
            _time = 0f;
            Owner.Respawn();
        }
    }

    public override void UpdateAnimation() { }

    public override void UpdateSwitchState() { }

    public override void UpdatePhysics() { }
}