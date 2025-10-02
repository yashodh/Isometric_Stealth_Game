using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandState : PlayerBaseState
{
    private float _vertAnim;
    private float _y;
    private bool _fast;

    private float _normalSpeed = 2.0f;
    private float _fastSpeed = 4.0f;
    private float _turnSpeed = 200.0f;
    private int _turnDir = 0;

    public override PlayerState State { get { return PlayerState.STANDUP; } set { } }

    public override void EnterState(Player player)
    {
        Owner = player;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (Owner.InputControl.WhistleTapped)
        {
            Owner.AudioControl.PlayOnce(AudioEnum.Whistle);
            Director.Instance.ProcessMessage(Message.Whistle, Owner.GetPosition());
        }
    }

    public override void UpdateAnimation()
    {
        _y = Owner.InputControl.MoveHeld.y;
        _fast = Owner.InputControl.SprintHeld;

        PlayerAnim anim = PlayerAnim.MoveVertical;
        _vertAnim = Owner.AnimationControl.GetFloat(anim);

        // Update movement Speed and Animation
        if (_y > 0 && _vertAnim < (_fast ? 1.0f : 0.5f))
        {
            Owner.AnimationControl.SetFloat(anim, _vertAnim + Time.deltaTime * 5.0f);
        }
        else if (_y < 0 && _vertAnim > (_fast ? -1.0f : -0.5f))
        {
            Owner.AnimationControl.SetFloat(anim, _vertAnim - Time.deltaTime * 5.0f);
        }
        else
        {
            if(_vertAnim > 0)
                Owner.AnimationControl.SetFloat(anim, _vertAnim - Time.deltaTime * 1.0f);
            else if(_vertAnim < 0)
                Owner.AnimationControl.SetFloat(anim, _vertAnim + Time.deltaTime * 1.0f);
        }

        // Update Turn Direction
        if (Owner.InputControl.MoveHeld.x > 0)
        {
            _turnDir = 1;
        }
        else if(Owner.InputControl.MoveHeld.x < 0)
        {
            _turnDir = -1;
        }
        else
        {
            _turnDir = 0;
        }
    }

    public override void UpdateSwitchState()
    {
        // if crouch button is tapped. Switch to crouch state
        if (Owner.InputControl.CrouchTapped)
        {
            Owner.StateMachine.SwitchState(PlayerState.CROUCH);
            return;
        }

        if (Owner.InputControl.CoverTapped)
        {
            if(Owner.TryCover())
            {
                Owner.StateMachine.SwitchState(PlayerState.COVER);
                return;
            }
        }

        if (Owner.InputControl.TakedownTapped)
        {
            if (Owner.TryAttack())
            {
                Owner.StateMachine.SwitchState(PlayerState.TAKEDOWN);
                return;
            }
        }

        if (_y > 0 && Owner.InputControl.SprintHeld || Owner.InputControl.VaultTapped)
        {
            if (Owner.Cover == null)
                return;

            if (!Owner.TryDetect("Cover", 0.25f, 1.0f, out RaycastHit hit, out float angle)) 
                return;

            if (angle > 50 || angle < -50)
                return;

            Owner.StateMachine.VaultAngle = Mathf.Abs(angle);

            Owner.StateMachine.SwitchState(PlayerState.VAULT);
            return;
        }
    }

    public override void UpdatePhysics()
    {
        Owner.Rb.MovePosition(Owner.Rb.position + _vertAnim * (_fast ? _fastSpeed : _normalSpeed) * Owner.transform.forward * Time.deltaTime);
        Owner.Rb.MoveRotation(Owner.Rb.rotation * Quaternion.Euler(Vector3.up * _turnDir * _turnSpeed * Time.deltaTime));
    }
}
