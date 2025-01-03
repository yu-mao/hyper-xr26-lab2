using UnityEngine;

public class VRControllerInput : MonoBehaviour, IControllerInput
{
    [SerializeField]
    private OVRInput.Controller controllerType;

    public Vector2 Joystick => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerType);

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsButtonPressed(ControllerButtonId buttonId)
    {
        return buttonId switch
        {
            ControllerButtonId.One => OVRInput.Get(OVRInput.Button.One, controllerType),
            ControllerButtonId.Two => OVRInput.Get(OVRInput.Button.Two, controllerType),
            _ => false
        };

    }

    public bool IsTriggerPressed()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controllerType);
    }

    public bool IsTriggerReleased()
    {
        return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controllerType);
    }
}