using System.Collections;
using UnityEngine;

public class PlayerTakedownState : PlayerBaseState
{
    PlayerAnim _chosenAnim;

    public override PlayerState State { get { return PlayerState.TAKEDOWN; } set { } }

    public override void EnterState(Player player)
    {
        Owner = player;

        // choose an animation as per type of takedown performed
        switch(Owner.TakeDownEnemy.Health.Context)
        {
            case HealthContext.BackTakedown:
                {
                    _chosenAnim = PlayerAnim.BackTakedown;
                }
                break;
            case HealthContext.FrontTakedown:
                {
                    _chosenAnim = PlayerAnim.FrontTakedown;
                }
                break;
        }

        Owner.AnimationControl.SetBool(_chosenAnim, true);
        Game.Instance.ToggleTakedownCamera(true);
    }


    public override void ExitState()
    {
        Game.Instance.ToggleTakedownCamera(false);
    }

    public override void UpdateState()
    {
       
    }

    public override void UpdateAnimation()
    {

    }

    public override void UpdateSwitchState()
    {
        if (Owner.AnimationControl.IsAnimationFinished(_chosenAnim, 0))
        {
            Owner.AnimationControl.SetBool(_chosenAnim, false);
            Owner.StateMachine.SwitchState(PlayerState.STANDUP);
            return;
        }
    }

    public override void UpdatePhysics()
    {

    }

}