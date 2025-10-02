using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _cam;

    public CameraStateMachine StateMachine;

    public void Init()
    {
        _cam = GetComponent<Camera>();

        StateMachine = new CameraStateMachine(this);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public Camera GetCamera()
    {
        return _cam;
    }
}
