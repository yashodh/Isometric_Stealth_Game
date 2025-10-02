using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl
{
    #region Input
    public PlayerInput playerInput;
    private InputAction _movement;
    private InputAction _crouch;
    private InputAction _sprint;
    private InputAction _cover;
    private InputAction _takedown;
    private InputAction _vault;
    private InputAction _whistle;
    #endregion

    #region Properties
    public Vector2 MoveHeld;
    public bool SprintHeld;
    public bool CrouchTapped;
    public bool CoverTapped;
    public bool TakedownTapped;
    public bool VaultTapped;
    public bool WhistleTapped;
    #endregion

    public PlayerInputControl()
    {
        playerInput = new PlayerInput();

        _movement = playerInput.Player.Movement;
        _movement.Enable();

        _crouch = playerInput.Player.Crouch;
        _crouch.Enable();

        _sprint = playerInput.Player.Sprint;
        _sprint.Enable();

        _cover = playerInput.Player.Cover;
        _cover.Enable();

        _takedown = playerInput.Player.Takedown;
        _takedown.Enable();

        _vault = playerInput.Player.Vault;
        _vault.Enable();

        _whistle = playerInput.Player.Whistle;
        _whistle.Enable();
    }

    public void ReadInput()
    {
        MoveHeld = _movement.ReadValue<Vector2>();
        SprintHeld = _sprint.ReadValue<float>() == 1.0f ? true : false;
        
        CrouchTapped = _crouch.triggered;
        CoverTapped = _cover.triggered;
        TakedownTapped = _takedown.triggered;
        VaultTapped = _vault.triggered;

        WhistleTapped = _whistle.triggered;
    }
}