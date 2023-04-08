using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputLoggerManager : MonoBehaviour
{
    [Header("Controller Mapping")]
    [SerializeField]
    private XRBaseController controller;

    [Header("Controller Actions")]
    [SerializeField]
    private InputActionProperty controllerSelect;

    public InputActionProperty ControllerSelect
    {
        get => controllerSelect;
        set => controllerSelect = value;
    }

    [SerializeField]
    private InputActionProperty controllerSelectValue;

    public InputActionProperty ControllerSelectValue
    {
        get => controllerSelectValue;
        set => controllerSelectValue = value;
    }

    [SerializeField]
    private InputActionProperty controllerPosition;

    public InputActionProperty ControllerPosition
    {
        get => controllerPosition;
        set => controllerPosition = value;
    }

    [SerializeField]
    private InputActionProperty controllerRotation;

    public InputActionProperty ControllerRotation
    {
        get => controllerRotation;
        set => controllerRotation = value;
    }

    [SerializeField]
    private float frequencyForDisplayingInputs = 0.25f;

    private void Start() => StartCoroutine(DisplayControllerInputValues());

    private void OnEnable()
    {
        controllerSelect.action.started += ControllerSelectStarted;
        controllerSelect.action.performed += ControllerSelectPerfomed;
    }

    private void OnDisable()
    {
        controllerSelect.action.started -= ControllerSelectStarted;
        controllerSelect.action.performed -= ControllerSelectPerfomed;
    }

    private void ControllerSelectStarted(InputAction.CallbackContext obj)
    {
        Logger.Instance.LogInfo("ControllerSelectStarted executed");
    }

    private void ControllerSelectPerfomed(InputAction.CallbackContext obj)
    {
        Logger.Instance.LogInfo("ControllerSelectPerfomed executed");
        controller.SendHapticImpulse(1.0f, 1.0f);
    }

    private IEnumerator DisplayControllerInputValues()
    {
        while (true)
        {
            if (ControllerSelect.action.enabled)
            {
                Logger.Instance.LogInfo($"controllerSelect: {ControllerSelect.action.IsPressed()}");
            }
            if (ControllerSelectValue.action.enabled)
            {
                Logger.Instance.LogInfo($"controllerSelectValue: {ControllerSelect.action.ReadValue<float>()}");
            }
            if (ControllerPosition.action.enabled)
            {
                Logger.Instance.LogInfo($"controllerPosition: {ControllerPosition.action.ReadValue<Vector3>()}");
            }
            if (ControllerRotation.action.enabled)
            {
                Logger.Instance.LogInfo($"controllerRotation: {ControllerRotation.action.ReadValue<Quaternion>()}");
            }
            yield return new WaitForSeconds(frequencyForDisplayingInputs);
        }
    }
}
