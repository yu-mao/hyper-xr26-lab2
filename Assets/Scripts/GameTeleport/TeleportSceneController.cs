using System;
using UnityEngine;

public class TeleportSceneController : MonoBehaviour
{
    [SerializeField] private float teleportSpeed = 2f;
    [SerializeField] private float teleportRayLength = 10f;
    [SerializeField] private LineRenderer teleportRay;
    
    private GameManager gameManager;
    private IInputProvider inputProvider;
    private IControllerInput leftController;
    private IControllerInput rightController;

    private bool isIntendToTeleport = false;
    private bool canTeleport = false;
    private bool isTeleporting = false;
    private Vector3 teleportDestination;
    
    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        inputProvider = gameManager.InputProvider;
        rightController = inputProvider.GetRightController();
        teleportRay.enabled = false;
        teleportRay.positionCount = 2;
    }

    private void Start()
    {
        if (gameManager == null)
        {
            Initialize(GameManager.BootstrapFromEditor());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // intend to teleport when moving right controller's joystick forward
        if (rightController.Joystick.y > 0.5f)
        {
            isIntendToTeleport = true;
        }
        else
        {
            isIntendToTeleport = false;
        }

        if (isIntendToTeleport)
        {
            if (CanTeleport())
            {
                VisualizeTeleportRay(true, rightController.GetTransform().position, 
                    teleportDestination, Color.green);
                if (rightController.IsTriggerPressed())
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                VisualizeTeleportRay(true, rightController.GetTransform().position,
                    rightController.GetTransform().position +
                    rightController.GetTransform().forward * teleportRayLength, Color.red);
            }
        }
        else
        {
            VisualizeTeleportRay(false, Vector3.zero, Vector3.zero, Color.red);
        }
    }

    private bool CanTeleport()
    {
        if (Physics.Raycast(rightController.GetTransform().position, 
                rightController.GetTransform().forward,
                out RaycastHit hit, teleportRayLength))
        {
            if (hit.collider.GetComponent<TeleportSurface>())
            {
                teleportDestination = hit.point;
                return true;
            }
        }

        return false;
    }

    private void VisualizeTeleportRay(bool isVisible, Vector3 startPosition, Vector3 endPosition, Color color)
    {
        teleportRay.enabled = isVisible;
        teleportRay.SetPosition(0, startPosition);
        teleportRay.SetPosition(1, endPosition);
        teleportRay.startColor = color;
        teleportRay.endColor = color;
    }
}
