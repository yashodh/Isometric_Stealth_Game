using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTakedownState : CameraBaseState
{
    CameraController Owner;

    Transform playerTransform;

    Vector3[] dirs =
        {   new Vector3 (1,1,1),
            new Vector3(-1,1,1),
            new Vector3(-1,1,-1),
            new Vector3(1,1,-1),
        };

    Vector3 Elevation = new Vector3(0, 1, 0);
    Vector3 chosenDir;

    public override CameraState State { get { return CameraState.Takedown; } set { } }

    public override void EnterState(CameraController camCont)
    {
        Owner = camCont;
        playerTransform = Player.Instance.gameObject.transform;

        Owner.transform.parent = playerTransform;

        chosenDir = Elevation;
        for (int i = 0; i < dirs.Length; i++)
        {
            if (Physics.Raycast(Player.Instance.GetPosition() + 0.15f * dirs[i], dirs[i], 2.0f))
            {
                //Debug.DrawLine(Player.Instance.GetPosition() + 0.15f * dirs[i], Player.Instance.GetPosition() + 1.15f * dirs[i], Color.red, 5.0f);
                continue;
            }

            //Debug.DrawLine(Player.Instance.GetPosition() + 0.15f * dirs[i], Player.Instance.GetPosition() + 1.5f * dirs[i], Color.green, 5.0f);
            chosenDir = dirs[i];
            break;
        }

        Time.timeScale = 0.5f;
    }

    public override void ExitState()
    {
        Time.timeScale = 1.0f;
    }

    public override void UpdateState()
    {
        Owner.transform.forward = -1 * chosenDir;
        Owner.transform.position = playerTransform.position + 1.5f * chosenDir;
    }
}
