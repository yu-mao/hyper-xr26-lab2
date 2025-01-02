using UnityEngine;

public class JoystickSceneController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float rotationTimeInterval = 0.5f; // min time interval between two rotations

    private GameManager gameManager;
    private IInputProvider inputProvider;
    private IControllerInput leftController;
    private IControllerInput rightController;
    private bool canRotate = true;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        inputProvider = gameManager.InputProvider;
        leftController = inputProvider.GetLeftController();
        rightController = inputProvider.GetRightController();
    }

    private void Update()
    {
        if (leftController.IsButtonPressed(ControllerButtonId.Two))
        {
            gameManager.GoToMenuScene();
        }

        var forward = inputProvider.GetHeadTransform().forward;
        forward.y = 0f;
        forward.Normalize();

        var right = inputProvider.GetHeadTransform().right;
        right.y = 0f;
        right.Normalize();

        forward *= rightController.Joystick.y;
        right *= rightController.Joystick.x;

        var movement = forward + right;
        movement *= movementSpeed * Time.deltaTime;

        inputProvider.GetRigTransform().position += movement;
        
        // Snap to turn
        float rotation = leftController.Joystick.x;
        if (rotation > 0.5f && canRotate)
        {
            Rotate(Vector3.up);
        }
        else if (rotation < -0.5f && canRotate)
        {
            Rotate(Vector3.down);
        }
        
    }

    private void Rotate(Vector3 direction)
    {
        inputProvider.GetRigTransform().Rotate(direction * 90f, Space.World);
        
        canRotate = false;
        Invoke(nameof(ResetRotation), rotationTimeInterval);
    }

    private void ResetRotation()
    {
        canRotate = true;
    }
}

