using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    float angle;
    float distSqr;
    RaycastHit hit;

    #region Properties
    float FOV;
    float alertDistance;
    float alertDistanceSqr;
    float chaseDistance;
    float chaseDistanceSqr;
    #endregion

    public override EnemyState State { get { return EnemyState.PATROL; } set { } }

    public override void EnterState(Enemy guard)
    {
        InitStateData();

        Owner = guard;
        Owner.Agent.speed = 0.7f;

        Owner.Agent.ResetPath();
        Owner.Spawner.PathController.Register(guard);

        Owner.EnableFov(1, chaseDistance, FOV);
        Owner.EnableFov(2, alertDistance, FOV);
    }

    public override void ExitState()
    {
        Owner.DisableFov(1);
        Owner.DisableFov(2);

        Owner.Spawner.PathController.Unregister();
    }

    public override void UpdateSwitchState()
    {
        // if within angle and distance, detect
        angle = Vector3.Angle(Player.Instance.GetPosition() - Owner.GetPosition(), Owner.GetForward());
        distSqr = Vector3.SqrMagnitude(Player.Instance.GetPosition() - Owner.GetPosition());

        if (angle >= -FOV / 2 && angle <= FOV / 2 && distSqr < alertDistanceSqr)
        {
            if (Physics.SphereCast(Owner.GetPosition(), 0.25f, Vector3.Normalize(Player.Instance.GetPosition() - Owner.GetPosition()), out hit, alertDistance))
            {
                if (hit.collider.tag != "Player")
                {
                    return;
                }
            }

            // if the hit was on the player
            // if distance is less than 1 , immediately chase, else go into alert state
            if (distSqr < chaseDistanceSqr)
            {
                Owner.StateMachine.SwitchState(EnemyState.ATTACK);
                return;
            }
            else
            {
                Owner.StateMachine.SwitchState(EnemyState.INVESTIGATE);
                return;
            }
        }
    }

    public override void UpdateAnimation()
    {
        EnemyAnim anim = EnemyAnim.Move;
        float move = Owner.AnimationControl.GetFloat(anim);

        if (Owner.Agent.hasPath)
        {
            if (move < 0.5f)
                Owner.AnimationControl.SetFloat(anim, move + Time.deltaTime * 1.0f);
        }
        else
        {
            if (move > 0)
                Owner.AnimationControl.SetFloat(anim, move - Time.deltaTime * 1.0f);
        }
    }

    private void InitStateData()
    {
        FOV = Game.Instance.GuardData.Patrol_FOV;

        alertDistance = Game.Instance.GuardData.Patrol_DistFar;
        alertDistanceSqr = Mathf.Pow(alertDistance, 2);

        chaseDistance = Game.Instance.GuardData.Patrol_DistNear;
        chaseDistanceSqr = Mathf.Pow(chaseDistance, 2);
    }
}
