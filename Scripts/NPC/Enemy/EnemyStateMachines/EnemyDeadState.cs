using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    float _time;

    float UpTime = 5.0f;

    public override EnemyState State { get { return EnemyState.DEAD; } set { } }

    public override void EnterState(Enemy guard)
    {
        Owner = guard;
        Owner.Agent.ResetPath();

        Owner.GetComponent<Collider>().enabled = false;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateSwitchState()
    {
        _time += Time.deltaTime;

        // wait to despawn the enemy
        if (_time > UpTime)
        {
            _time = 0f;
            Owner.Despawn();
        }
    }

    public override void UpdateAnimation()
    {

    }
}
