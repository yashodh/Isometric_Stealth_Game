using System.Collections;
using UnityEngine;

public class EnemyAnimationControl
{
    private Animator _animator;

    public EnemyAnimationControl(Animator a)
    {
        _animator = a;
    }

    public float GetFloat(EnemyAnim anim)
    {
        return _animator.GetFloat(anim.ToString());
    }

    public void SetFloat(EnemyAnim anim, float f)
    {
        _animator.SetFloat(anim.ToString(), f);
    }

    public void SetInt(EnemyAnim anim, int i)
    {
        _animator.SetInteger(anim.ToString(), i);
    }

    public void SetTrigger(EnemyAnim anim)
    {
        _animator.SetTrigger(anim.ToString());
    }
}

public enum EnemyAnim
{
    Move,
    Direction,
    Melee,
    Shoot,
    FrontTakedown,
    BackTakedown,
    ExplosiveTakedown
}