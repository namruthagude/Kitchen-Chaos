using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDING = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down, 
        Move_Left, 
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause
    }

    private PlayerInputs playerInputs;

    private void Awake()
    {
        Instance = this;
        playerInputs = new PlayerInputs();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING))
        {
            playerInputs.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));
        }
        playerInputs.Player.Enable();
        playerInputs.Player.Interact.performed += Interact_performed;
        playerInputs.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputs.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputs.Player.Interact.performed -= Interact_performed;
        playerInputs.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputs.Player.Pause.performed -= Pause_performed;

        playerInputs.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputs.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputs.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputs.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputs.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputs.Player.Move.bindings[1].ToDisplayString();
            case Binding.Interact:
                return playerInputs.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Alternate:
                return playerInputs.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputs.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void ReBinding(Binding binding, Action OnActionRebound)
    {
        playerInputs.Player.Disable();

        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputs.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputs.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputs.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputs.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputs.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alternate:
                inputAction = playerInputs.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputs.Player.Pause;
                bindingIndex = 0;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete( callback =>
            {
                callback.Dispose();
                playerInputs.Player.Enable();
                OnActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDING, playerInputs.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}
