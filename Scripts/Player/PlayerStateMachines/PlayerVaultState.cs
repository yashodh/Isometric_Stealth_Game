using System.Collections;
using UnityEngine;

public class PlayerVaultState : PlayerBaseState
{
    float scale = 0.5f;
    Vector3 height = new Vector3(0, 0.5f, 0);

    float _forwardSpeed = 1.5f;

    public override PlayerState State { get { return PlayerState.VAULT; } set { } }

    public override void EnterState(Player player)
    {
        Owner = player;

        Owner.Rb.isKinematic = true;
        //Owner.AnimationControl.ToggleRootMotion(true);
        Owner.GetComponent<CapsuleCollider>().height *= scale;
        Owner.GetComponent<CapsuleCollider>().center += height;

        _forwardSpeed = (Owner.StateMachine.VaultAngle < 25f ? 1.8f : 2.0f);
        Owner.AnimationControl.SetBool(PlayerAnim.Vault, true);
    }

    public override void ExitState()
    {
        Owner.GetComponent<CapsuleCollider>().center -= height;
        Owner.GetComponent<CapsuleCollider>().height /= scale;
        //Owner.AnimationControl.ToggleRootMotion(false);
        Owner.Rb.isKinematic = false;
    }

    public override void UpdateState()
    {

    }

    public override void UpdateSwitchState()
    {
        if (Owner.AnimationControl.IsAnimationFinished(PlayerAnim.Vault, 0))
        {
            Owner.AnimationControl.SetBool(PlayerAnim.Vault, false);
            Owner.StateMachine.SwitchState(PlayerState.STANDUP);
            return;
        }
    }

    public override void UpdateAnimation()
    {
        
    }

    public override void UpdatePhysics()
    {
        Owner.Rb.MovePosition(Owner.Rb.position + _forwardSpeed * Owner.transform.forward * Time.deltaTime);
    }
}