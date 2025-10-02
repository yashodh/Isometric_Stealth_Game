using UnityEngine;

public class CameraIsometricState : CameraBaseState
{
    CameraController Owner;

    Transform playerTransform;

    Vector3 Elevation = new Vector3(0, 4, 0);

    public override CameraState State { get { return CameraState.Isometric; } set { } }

    public override void EnterState(CameraController camCont)
    {
        Owner = camCont;
        playerTransform = Player.Instance.gameObject.transform;

        Owner.transform.parent = null;
        Owner.transform.forward = Quaternion.AngleAxis(-50.0f, Vector3.forward) * Vector3.right;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        Owner.transform.position = playerTransform.position - 5.0f * Vector3.right + Elevation;

        if (Input.GetKeyDown(KeyCode.Q))
            Owner.StateMachine.SwitchState(CameraState.ThirdPerson);
    }
}
