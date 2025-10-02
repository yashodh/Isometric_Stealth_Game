using System.Collections;
using UnityEngine;

public class PlayerCoverState : PlayerBaseState
{
    private float _horAnim;
    private float _horInput;

    private float _normalSpeed = 1.0f;
    private int _dir;

    public override PlayerState State { get { return PlayerState.COVER; } set { } }

    public override void EnterState(Player player)
    {
        Owner = player;
        Owner.AnimationControl.SetBool(PlayerAnim.Cover, true);
    }

    public override void ExitState()
    {
        Owner.AnimationControl.SetBool(PlayerAnim.Cover, false);
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
        _horInput = Owner.InputControl.MoveHeld.x;

        PlayerAnim anim = PlayerAnim.MoveHorizontal;
        _horAnim = Owner.AnimationControl.GetFloat(PlayerAnim.MoveHorizontal);

        if (_dir > 0)
        {
            if (_horInput > 0 && _horAnim < 1.0f)
            {
                Owner.AnimationControl.SetFloat(anim, _horAnim + Time.deltaTime * 5.0f);
            }
            else if (_horInput < 0 && _horAnim > -1.0f)
            {
                Owner.AnimationControl.SetFloat(anim, _horAnim - Time.deltaTime * 5.0f);
            }
            else
            {
                if (_horAnim > 0)
                    Owner.AnimationControl.SetFloat(anim, _horAnim - Time.deltaTime * 1.0f);
                else if (_horAnim < 0)
                    Owner.AnimationControl.SetFloat(anim, _horAnim + Time.deltaTime * 1.0f);
            }
        }
        else if(_dir < 0)
        {
            if (_horInput < 0 && _horAnim < 1.0f)
            {
                Owner.AnimationControl.SetFloat(anim, _horAnim + Time.deltaTime * 5.0f);
            }
            else if (_horInput > 0 && _horAnim > -1.0f)
            {
                Owner.AnimationControl.SetFloat(anim, _horAnim - Time.deltaTime * 5.0f);
            }
            else
            {
                if (_horAnim > 0)
                    Owner.AnimationControl.SetFloat(anim, _horAnim - Time.deltaTime * 1.0f);
                else if (_horAnim < 0)
                    Owner.AnimationControl.SetFloat(anim, _horAnim + Time.deltaTime * 1.0f);
            }
        }
    }

    public override void UpdateSwitchState()
    {
        if (Owner.InputControl.CoverTapped)
        {
            Owner.StateMachine.SwitchState(PlayerState.STANDUP);
            return;
        }
    }

    public override void UpdatePhysics()
    {
        //float angle = Vector3.Angle(Owner.transform.forward, Owner.hit.normal);
        //if (angle > 5f)
        //{
        //    Owner.Rb.MoveRotation(Owner.Rb.rotation * Quaternion.Euler(-1f * Vector3.up * 300.0f * Time.deltaTime));
        //}
        //else
        //{
        //    float ang = Vector3.Angle(Vector3.right, Owner.hit.normal);

        //    if (ang > 90f)
        //    {
        //        _dir = -1;
        //    }
        //    else
        //    {
        //        _dir = 1;
        //    }

        //    Owner.Rb.MovePosition(Owner.Rb.position + _horAnim * _normalSpeed *  Owner.transform.right * Time.deltaTime);
        //}        
    }
}