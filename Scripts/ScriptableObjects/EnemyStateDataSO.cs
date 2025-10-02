using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStateData", menuName = "ScriptableObjects/EnemyStateData", order = 1)]
public class EnemyStateDataSO : ScriptableObject
{
    [Header("Idle State")]
    public float Idle_FOV = 50.0f;
    public float Idle_Dist = 2.0f;
    public float Idle_StateTime = 3.0f;

    [Header("LookAround State")]
    public float LookAround_FOV = 50.0f;
    public float LookAround_Dist = 2.0f;
    public float LookAround_TurnAngle = 180.0f;
    public float LookAround_TurnTime = 5.0f;

    [Header("Patrol State")]
    public float Patrol_FOV = 90.0f;
    public float Patrol_DistNear = 1.0f;
    public float Patrol_DistFar = 2.0f;

    [Header("Tailgate State")]
    public float Tailgate_DistNear = 2.0f;
    public float Tailgate_DistFar = 4.0f;

    [Header("Investigate State")]
    public float Investigate_FOV = 180.0f;
    public float Investigate_DistNear = 2.0f;
    public float Investigate_DistFar = 4.0f;
    public float Investigate_StateTime = 5.0f;

    [Header("Shoot State")]
    public float Shoot_FOV = 50.0f;
    public float Shoot_DistNear = 2.0f;
    public float Shoot_DistFar = 3.0f;
}
