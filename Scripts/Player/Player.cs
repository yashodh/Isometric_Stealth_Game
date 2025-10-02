using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance = null;

    [SerializeField]
    private Animator _animator;

    public Rigidbody Rb;
    public AudioController AudioControl;

    [Header("Raycast Starters")]
    public GameObject High;
    public GameObject Mid;
    public GameObject Low;

    private Transform _transform = null;
    private bool _isInitialized = false;

    #region PROPERTIES
    public Health Health;
    public PlayerStateMachine StateMachine;
    public PlayerInputControl InputControl;
    public PlayerAnimationControl AnimationControl;
    #endregion

    [HideInInspector]
    public Enemy TakeDownEnemy;
    [HideInInspector]
    public Cover Cover;

    public void Init()
    {
        Instance = this;
        _transform = gameObject.transform;

        // Order of Initialization is very important. Don't mess with it.
        Health = new Health();
        InputControl = new PlayerInputControl();
        AnimationControl = new PlayerAnimationControl(_animator);
        StateMachine = new PlayerStateMachine(this);

        _isInitialized = true;

        AudioControl.Play(AudioEnum.Breath);
    }

    public void Despawn()
    {
        Destroy(this);
    }

    public void Respawn()
    {
        Init();
    }

    private void Update()
    {
        if (!_isInitialized)
            return;

        // order is important. don't mess with it.
        InputControl.ReadInput();
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    public Vector3 GetPosition()
    {
        return _transform.position;
    }

    public bool IsCurrentState(PlayerState state)
    {
        return StateMachine.CurrState == state;
    }


    //public RaycastHit hit;
    public bool TryCover()
    {       
        if (Physics.SphereCast(transform.position + 0.15f * transform.forward, 0.25f, transform.forward, out RaycastHit hit, 2.0f))
        {
            if (hit.collider.tag == "Cover")
            {
                Debug.DrawLine(transform.position + 0.15f * transform.forward, hit.point, Color.red, 1.0f);
                Debug.Log("Cover Detected");

                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue, 1.0f);

                transform.position = hit.point + 0.15f * hit.normal;

                return true;
            }
        }

        return false;
    }

    public bool TryAttack()
    {
        Debug.DrawLine(transform.position + 0.15f * transform.forward, transform.position + 1.0f * transform.forward, Color.red, 5.0f);

        if (Physics.SphereCast(transform.position + 0.15f * transform.forward, 0.25f, transform.forward, out RaycastHit hit, 1.0f))
        {
            if (hit.collider.tag == "Enemy")
            {
                Enemy g = hit.collider.gameObject.GetComponent<Enemy>();

                if (g == null)
                    return false;

                Debug.DrawLine(transform.position + 0.15f * transform.forward, hit.point, Color.red, 1.0f);

                float angle = Vector3.Angle(g.transform.forward, Player.Instance.transform.forward);

                if (angle < 90f && angle > -90f)
                {
                    g.DealDamage(100, HealthContext.BackTakedown);
                }
                else
                {
                    g.DealDamage(100, HealthContext.FrontTakedown);
                }

                TakeDownEnemy = g;
                return true;
            }
        }

        return false;
    }

    public bool TryDetect(string tag, float radius, float distance, out RaycastHit detectHit, out float angle)
    {
        angle = 0;

        if (Physics.SphereCast(Low.transform.position, radius, transform.forward, out detectHit, distance))
        {
            if (detectHit.collider.tag == tag)
            {
                angle = Vector3.Angle(-1 * detectHit.normal, Player.Instance.transform.forward);
                return true;
            }
        }

        return false;
    }

    public void DealDamage(int damage, HealthContext context)
    {
        Health.DealDamage(damage, context);
        AudioControl.PlayOnce(AudioEnum.Hurt);
        PlayDamageAnimation();
    }

    private void PlayDamageAnimation()
    {
        switch (Health.Context)
        {
            case HealthContext.Ranged:
                AnimationControl.SetTrigger(PlayerAnim.RangedDamage);
                break;
            case HealthContext.Melee:
                AnimationControl.SetTrigger(PlayerAnim.MeleeDamage);
                break;
        }
    }

    // only for animation event. Don't use anywhere else
    public void PlayAnimationEventAudio(AudioEnum type)
    {
        AudioControl.PlayOnce(type);
    }

    private void OnDrawGizmos()
    {
        if (transform == null || Health == null)
            return;

        Gizmos.DrawWireSphere(transform.position + 4f * transform.forward, 0.1f);
        Handles.Label(transform.position + new Vector3(0, 1f, 0), Health.GetHealth().ToString());
    }
}
