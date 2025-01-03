using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyboardMouseControllerInput : MonoBehaviour, IControllerInput
{
    [SerializeField]
    private bool isLeftController;
    [SerializeField]
    private KeyControl buttonA;
    [SerializeField]
    private InputAction buttonB;

    public Vector2 Joystick { get; private set; }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsButtonPressed(ControllerButtonId buttonId)
    {
        return buttonId switch
        {
            ControllerButtonId.One => isLeftController ? Keyboard.current.digit1Key.isPressed : Keyboard.current.digit3Key.isPressed,
            ControllerButtonId.Two => isLeftController ? Keyboard.current.digit2Key.isPressed : Keyboard.current.digit4Key.isPressed,
            _ => false
        };
    }

    public bool IsTriggerPressed()
    {
        if (isLeftController)
        {
            return Mouse.current.leftButton.wasPressedThisFrame;
        }
        else
        {
            return Mouse.current.rightButton.wasPressedThisFrame;
        }
    }

    public bool IsTriggerReleased()
    {
        if (isLeftController)
        {
            return Mouse.current.leftButton.wasReleasedThisFrame;
        }
        else
        {
            return Mouse.current.rightButton.wasReleasedThisFrame;
        }
    }

    private void Update()
    {
        var joystick = Vector2.zero;
        var kb = Keyboard.current;

        if (isLeftController)
        {
            joystick.x += kb.lKey.value;
            joystick.x -= kb.jKey.value;
            joystick.y += kb.iKey.value;
            joystick.y -= kb.kKey.value;
        }
        else
        {
            joystick.x += kb.rightArrowKey.value;
            joystick.x -= kb.leftArrowKey.value;
            joystick.y += kb.upArrowKey.value;
            joystick.y -= kb.downArrowKey.value;
        }

        Joystick = joystick.normalized;
    }
}
