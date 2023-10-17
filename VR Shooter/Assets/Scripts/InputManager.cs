using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Action<Boolean, float> RightTriggerButtonAction;
    public static Action<Boolean, float> LeftTriggerButtonAction;
    public static Action<float> RightXbuttonAction;
    public static Action<float> LeftXbuttonAction;
    XRIDefaultInputActions inputAction = null;

    private void Awake()
    {
        inputAction = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.XRIRightHandInteraction.ActivateValue.started += OnTriggerButtonRightDown;
        inputAction.XRIRightHandInteraction.ActivateValue.performed += OnTriggerButtonRight;
        inputAction.XRIRightHandInteraction.ActivateValue.canceled += OntriggerButtonRightUp;
        inputAction.XRILeftHandInteraction.ActivateValue.started += OnTriggerButtonLeftDown;
        inputAction.XRILeftHandInteraction.ActivateValue.performed += OnTriggerButtonLeft;
        inputAction.XRILeftHandInteraction.ActivateValue.canceled += OntriggerButtonLeftUp;

        inputAction.XRIRightHandInteraction.Primary.performed += OnRightXbutton;
        inputAction.XRIRightHandInteraction.Primary.canceled += OnRightXbutton;
        inputAction.XRILeftHandInteraction.Primary.performed += OnLeftXbutton;
        inputAction.XRILeftHandInteraction.Primary.canceled += OnLeftXbutton;
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.XRIRightHandInteraction.ActivateValue.started -= OnTriggerButtonRightDown;
        inputAction.XRIRightHandInteraction.ActivateValue.performed -= OnTriggerButtonRight;
        inputAction.XRIRightHandInteraction.ActivateValue.canceled -= OntriggerButtonRightUp;
        inputAction.XRILeftHandInteraction.ActivateValue.started -= OnTriggerButtonLeftDown;
        inputAction.XRILeftHandInteraction.ActivateValue.performed -= OnTriggerButtonLeft;
        inputAction.XRILeftHandInteraction.ActivateValue.canceled -= OntriggerButtonLeftUp;

        inputAction.XRIRightHandInteraction.Primary.performed -= OnRightXbutton;
        inputAction.XRIRightHandInteraction.Primary.canceled -= OnRightXbutton;
        inputAction.XRILeftHandInteraction.Primary.performed -= OnLeftXbutton;
        inputAction.XRILeftHandInteraction.Primary.canceled -= OnLeftXbutton;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame


    void OnTriggerButtonRightDown(InputAction.CallbackContext value)
    {
        if (RightTriggerButtonAction != null)
            RightTriggerButtonAction?.Invoke(true, value.ReadValue<float>());
    }

    void OnTriggerButtonRight(InputAction.CallbackContext value)
    {
        if (RightTriggerButtonAction != null)
            RightTriggerButtonAction?.Invoke(true, value.ReadValue<float>());
    }

    void OntriggerButtonRightUp(InputAction.CallbackContext value)
    {
        RightTriggerButtonAction?.Invoke(false, value.ReadValue<float>());
    }

    void OnTriggerButtonLeftDown(InputAction.CallbackContext value)
    {
        LeftTriggerButtonAction?.Invoke(true, value.ReadValue<float>());
    }

    void OnTriggerButtonLeft(InputAction.CallbackContext value)
    {
        LeftTriggerButtonAction?.Invoke(true, value.ReadValue<float>());
    }

    void OntriggerButtonLeftUp(InputAction.CallbackContext value)
    {
        LeftTriggerButtonAction?.Invoke(false, value.ReadValue<float>());
    }

    void OnLeftXbutton(InputAction.CallbackContext value)
    {
        LeftXbuttonAction?.Invoke(value.ReadValue<float>());
    }

    void OnRightXbutton(InputAction.CallbackContext value)
    {
        RightXbuttonAction?.Invoke(value.ReadValue<float>());
    }
}