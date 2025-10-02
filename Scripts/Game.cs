using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Player Player;
    public Transform StartLocation;
    public CameraController CamController;
    public Director Director;
    public EnemyStateDataSO GuardData;
    public AudioManager Audio;

    public static Game Instance;

    private void Awake()
    {
        Instance = this;

        // Order of Initialization is very important. Don't mess with it.
        Audio.Init();

        Player = Instantiate(Player, StartLocation.position, Quaternion.identity);
        Player.Init();

        CamController.Init();
        Director.Init();
    }

    
    public void ToggleTakedownCamera(bool t)
    {
        if (t)
        {
            CamController.StateMachine.SwitchState(CameraState.Takedown);
        }
        else
        {
            CamController.StateMachine.SwitchState(CamController.StateMachine.PrevState);
        }        
    }
}
