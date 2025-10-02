using System.Collections;
using UnityEngine;

public class EnemyHideState : EnemyBaseState
{
    public override EnemyState State { get { return EnemyState.HIDE; } set { } }

    public override void EnterState(Enemy guard)
    {
        Owner = guard;
    }

    public override void ExitState()
    {

    }

    public override void UpdateSwitchState()
    {

    }

    public override void UpdateAnimation()
    {

    }
}