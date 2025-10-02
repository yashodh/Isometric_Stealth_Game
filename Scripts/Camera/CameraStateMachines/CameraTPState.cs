using UnityEngine;

public class CameraTPState : CameraBaseState
{
    CameraController Owner;

    Transform playerTransform;

    Vector3 Elevation = new Vector3(0, 1.2f, 0);

    public override CameraState State { get { return CameraState.ThirdPerson; } set { } }

    public override void EnterState(CameraController camCont)
    {
        Owner = camCont;
        playerTransform = Player.Instance.gameObject.transform;

        Owner.transform.parent = playerTransform;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        Owner.transform.forward = Quaternion.AngleAxis(25.0f, playerTransform.right) * playerTransform.forward;
        Owner.transform.position = playerTransform.position - 1.5f * playerTransform.forward + Elevation;

        if (Input.GetKeyDown(KeyCode.Q))
            Owner.StateMachine.SwitchState(CameraState.Isometric);
    }
}
