using UnityEngine;

public interface IControllerInput
{
    Transform GetTransform();

    bool IsTriggerPressed();
    bool IsTriggerReleased();
    
    bool IsButtonPressed(ControllerButtonId buttonId);

    Vector2 Joystick { get; }
}
