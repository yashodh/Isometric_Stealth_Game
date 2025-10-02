using UnityEngine;

public abstract class EnemyBaseState
{
    public Enemy Owner;

    public abstract EnemyState State { get; set; }

    public abstract void EnterState(Enemy guard);

    public abstract void ExitState();

    public abstract void UpdateSwitchState();

    public abstract void UpdateAnimation();
}

public enum EnemyState
{
    IDLE,
    PATROL,
    INVESTIGATE,
    ATTACK,
    TURN,
    HIDE,
    DEAD
}
