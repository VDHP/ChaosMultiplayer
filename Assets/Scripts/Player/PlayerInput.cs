using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputAction inputActions;
    public event EventHandler OnInteractAction;
    public event Action OnInteractAlternate;
    public event Action OnPauseAction;
    public event Action OnRebinding;

    const string PLAYER_PRERS_BINDINGS = "InputBindings";

    public static PlayerInput Instance { get; private set; }



    public enum Binding
    {
        Move_Up,
        Move_Down, Move_Left, Move_Right,
        Interact,InteractAlt,
        Pause
    }
    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PRERS_BINDINGS))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PRERS_BINDINGS));
        } 

        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        inputActions.Player.Pause.performed += Pause_performed;

    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke();
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternate?.Invoke();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetInputPlayerNormalized()
    {
        Vector2 inputPlayer = inputActions.Player.Move.ReadValue<Vector2>();
        inputPlayer = inputPlayer.normalized;
        return inputPlayer;
    }
    public string GetBindingText(Binding binding)
    {
        switch(binding)
        {
            default:
            case Binding.Interact:
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Left:
                return inputActions.Player.Move.bindings[5].ToDisplayString();
            case Binding.Move_Right:
                return inputActions.Player.Move.bindings[7].ToDisplayString();
        }
    }
    public void Rebinding(Binding binding, Action onActionRebound)
    {
        inputActions.Player.Disable();

        InputAction inputActionPlayer;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputActionPlayer = inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputActionPlayer = inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Left:
                inputActionPlayer = inputActions.Player.Move;
                bindingIndex = 5;
                break;
            case Binding.Move_Right:
                inputActionPlayer = inputActions.Player.Move;
                bindingIndex = 7;
                break;
            case Binding.Interact:
                inputActionPlayer = inputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputActionPlayer = inputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputActionPlayer = inputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputActionPlayer.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            inputActions.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PLAYER_PRERS_BINDINGS, inputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnRebinding?.Invoke();
        }).Start();
    }
}
