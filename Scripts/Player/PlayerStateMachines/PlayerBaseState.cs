using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState 
{
    public Player Owner;

    public abstract PlayerState State { get; set; }

    // Methods
    public abstract void EnterState(Player player);
    public abstract void ExitState();
    public abstract void UpdateState();

    public abstract void UpdateAnimation();
    public abstract void UpdateSwitchState();
    public abstract void UpdatePhysics();
}


public enum PlayerState
{
    STANDUP,
    CROUCH,
    VAULT,
    COVER,
    TAKEDOWN,
    DEAD,
}
