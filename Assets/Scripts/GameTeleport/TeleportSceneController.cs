using System;
using UnityEngine;

public class TeleportSceneController : MonoBehaviour
{
    [SerializeField] private float teleportSpeed = 2f;
    [SerializeField] private float teleportRayLength = 10f;
    [SerializeField] private LineRenderer teleportRay;
    [SerializeField] private Color validTeleportRayColor = Color.green;
    [SerializeField] private Color invalidTeleportRayColor = Color.red;
    
    private GameManager gameManager;
    private IInputProvider inputProvider;
    private IControllerInput leftController;
    private IControllerInput rightController;

    private bool isIntendToTeleport = false;
    private bool isAbleToTeleport = false;
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
        if (rightController.Joystick.y > 0.5f) isIntendToTeleport = true;
        else isIntendToTeleport = false;

        if (isIntendToTeleport && !isTeleporting)
        {
            isAbleToTeleport = CheckTeleportationFeasibility();
            if (isAbleToTeleport && rightController.IsTriggerPressed())
            {
                // isTeleporting = true;
                
            }
        }
        
        UpdateTeleportRayVisual();
    }

    private bool CheckTeleportationFeasibility()
    {
        if (Physics.Raycast(rightController.GetTransform().position, 
                rightController.GetTransform().forward,
                out RaycastHit hit, teleportRayLength))
        {
            if (hit.collider.TryGetComponent(out TeleportSurface teleportSurface))
            {
                teleportDestination = hit.point;
                return true;
            }
        }
        return false;
    }

    private void UpdateTeleportRayVisual()
    {
        if (isIntendToTeleport && !isTeleporting)
        {
            if (isAbleToTeleport)
            {
                DrawTeleportRay(true, rightController.GetTransform().position, 
                    teleportDestination, validTeleportRayColor);
            }
            else
            {
                DrawTeleportRay(true, rightController.GetTransform().position,
                    rightController.GetTransform().position +
                    rightController.GetTransform().forward * teleportRayLength, 
                    invalidTeleportRayColor);
            }
        }
        else 
        {
            DrawTeleportRay(false, Vector3.zero, Vector3.zero, invalidTeleportRayColor);
        }
    }
    
    private void DrawTeleportRay(bool isVisible, Vector3 startPosition, Vector3 endPosition, Color color)
    {
        teleportRay.enabled = isVisible;
        teleportRay.SetPosition(0, startPosition);
        teleportRay.SetPosition(1, endPosition);
        teleportRay.startColor = color;
        teleportRay.endColor = color;
    }

    private void SmoothlyTeleport()
    {
        throw new NotImplementedException();
    }
}
