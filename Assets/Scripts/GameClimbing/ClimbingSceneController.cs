using System;
using UnityEngine;

public class ClimbingSceneController : MonoBehaviour
{
    private GameManager gameManager;
    private IInputProvider inputProvider;
    private IControllerInput leftController;
    private IControllerInput rightController;
    private Vector3 prevClimbingPosition;
    private bool isClimbing = false;
    
    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        inputProvider = gameManager.InputProvider;
        leftController = inputProvider.GetLeftController();
        rightController = inputProvider.GetRightController();
    }

    private void Start()
    {
        if (gameManager == null)
        {
            Initialize(GameManager.BootstrapFromEditor());
        }
    }
    
    private void Update()
    {
        if (leftController.IsButtonPressed(ControllerButtonId.Two))
        {
            gameManager.GoToMenuScene();
        }
        
        if (rightController.IsTriggerPressed())
        {
            prevClimbingPosition = rightController.GetTransform().position;
            isClimbing = true;
        }
        else if (rightController.IsTriggerReleased())
        {
            isClimbing = false;
        }

        if (isClimbing) Climb(prevClimbingPosition, rightController.GetTransform().position);
    }

    private void Climb(Vector3 prevPosition, Vector3 currPosition)
    {
        Vector3 movement = prevPosition - currPosition;
        inputProvider.GetRigTransform().position += movement;
        prevClimbingPosition = currPosition;
    }
}
