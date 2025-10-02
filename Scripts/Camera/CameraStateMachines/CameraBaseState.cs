using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraBaseState
{
    public abstract CameraState State { get; set; }
    public abstract void EnterState(CameraController camCont);
    public abstract void ExitState();
    public abstract void UpdateState();
}

public enum CameraState
{
    ThirdPerson,
    Isometric,
    Takedown
}
