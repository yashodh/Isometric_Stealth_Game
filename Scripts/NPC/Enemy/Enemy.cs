using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;
    public NavMeshAgent Agent => _agent;

    [SerializeField]
    public Animator _animator;

    public GameObject ShootVFXPrefab;
    private GameObject _shootVfxObj;

    public MeshGenerator FOV1;
    public MeshGenerator FOV2;

    private EnemySpawner _spawner;
    public EnemySpawner Spawner { get { return _spawner; } set { _spawner = value; } }

    #region PROPERTIES
    public Health Health;
    public EnemyStateMachine StateMachine;
    public EnemyAnimationControl AnimationControl;
    #endregion

    public Vector3 LocationToInvestigate;

    private void OnEnable()
    {
        Health = new Health();
        AnimationControl = new EnemyAnimationControl(_animator);
        StateMachine = new EnemyStateMachine(this);

        Director.Instance.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        Director.Instance.UnregisterEnemy(this);   
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetForward()
    {
        return transform.forward;
    }

    public EnemyState GetCurrentState()
    {
        return StateMachine.CurrState;
    }
    public bool IsCurrentState(EnemyState state)
    {
        return StateMachine.CurrState == state;
    }

    public void Despawn()
    {
        Destroy(gameObject);
        Destroy(_shootVfxObj);
    }

    public void DrawDetectionArea(float angle, float multi, Color color, int split = 9)
    {
        Vector3 lenVec = multi * transform.forward;
        float absAngle = Mathf.Abs(angle);
        float dAngle = 2 * absAngle / split;

        for (int i = 0; i < split; i++)
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(absAngle - i * dAngle, Vector3.up) * lenVec, color, 0.01f);
        }
    }

    public bool IsPlayerInFov(float fieldOfView, float VisibleDistance)
    {
        // is player is dead return early
        if (Player.Instance.Health.IsDead)
            return false;

        float angle = Vector3.Angle(Player.Instance.GetPosition() - GetPosition(), GetForward());
        float distSqr = Vector3.SqrMagnitude(Player.Instance.GetPosition() - GetPosition());


        if (angle <= fieldOfView / 2 && angle >= -fieldOfView / 2 && distSqr < VisibleDistance * VisibleDistance)
        {
            return true;
        }

        return false;
    }

    public void EnableFov(int num, float dist, float angle)
    {
        MeshGenerator FOV = num == 1 ? FOV1 : FOV2;
        FOV.CreateArc(transform, dist, angle);
    }

    public void DisableFov(int num)
    {
        MeshGenerator FOV = num == 1 ? FOV1 : FOV2;
        FOV.ClearMesh();
    }

    // should be in a class of it's own
    public void PlayShootVfx()
    {
        if (_shootVfxObj != null)
            Destroy(_shootVfxObj);

        _shootVfxObj = Instantiate(ShootVFXPrefab, transform.position + new Vector3(0, 0.6f, 0) + transform.forward, transform.rotation);
    }

    public void DealDamage(int damage, HealthContext context)
    {
        Health.DealDamage(damage, context);
        PlayDamageAnimation();
    }

    private void PlayDamageAnimation()
    {
        switch (Health.Context)
        {
            case HealthContext.BackTakedown:
                {
                    transform.forward = Player.Instance.transform.forward;
                    transform.position = Player.Instance.GetPosition() + 0.5f * Player.Instance.transform.forward;

                    AnimationControl.SetTrigger(EnemyAnim.BackTakedown);
                    Director.Instance.ProcessMessage(Message.NormalEnemyDeath, GetPosition());
                }
                break;
            case HealthContext.FrontTakedown:
                {
                    transform.forward = -1 * Player.Instance.transform.forward;
                    transform.position = Player.Instance.GetPosition() + 0.6f * Player.Instance.transform.forward;

                    AnimationControl.SetTrigger(EnemyAnim.FrontTakedown);
                    Director.Instance.ProcessMessage(Message.NormalEnemyDeath, GetPosition());
                }
                break;
            case HealthContext.ExplosiveTakedown:
                {
                    AnimationControl.SetTrigger(EnemyAnim.ExplosiveTakedown);
                }
                break;
        }
    }

    public void InvestigateLocation(Vector3 location)
    {
        LocationToInvestigate = location;
        StateMachine.SwitchState(EnemyState.INVESTIGATE);
    }
}

