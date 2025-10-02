using System.Collections;
using UnityEngine;

// wrapper to write my own functions 
public class PlayerAnimationControl
{
    private Animator _animator;

    public PlayerAnimationControl(Animator a)
    {
        _animator = a;
    }

    public float GetFloat(PlayerAnim anim)
    {
        return _animator.GetFloat(anim.ToString());
    }

    public void SetFloat(PlayerAnim anim, float f)
    {
        _animator.SetFloat(anim.ToString(), f);
    }

    public void SetBool(PlayerAnim anim, bool b)
    {
        _animator.SetBool(anim.ToString() , b);
    }

    public void SetTrigger(PlayerAnim anim)
    {
        _animator.SetTrigger(anim.ToString());
    }

    public bool IsAnimationFinished(PlayerAnim anim, int layer)
    {
        bool isName = _animator.GetCurrentAnimatorStateInfo(layer).IsName(anim.ToString());
        bool isOver = _animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > 1;

        return isName && isOver;
    }

    public void ToggleRootMotion(bool toggle)
    {
        _animator.applyRootMotion = toggle;       
    }

    public Vector3 GetPosition()
    {
        return _animator.transform.position;    
    }

}

public enum PlayerAnim
{
    MoveVertical,
    MoveHorizontal,
    Crouch,
    Cover,
    Vault,
    BackTakedown,
    FrontTakedown,
    Throw,
    MeleeDamage,
    RangedDamage,
    DeathFallback,
    DeathExplosion,
}