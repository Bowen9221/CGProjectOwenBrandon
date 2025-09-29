using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class TempVentScript : MonoBehaviour
{
    private ThirdPersonActions playerActionsAsset;
    private ThirdPersonMovement playerMovementSc;


    private void Awake()
    {
        playerActionsAsset = new ThirdPersonActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovementSc = FindAnyObjectByType<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovementSc.isHuman && playerActionsAsset.Player.Interact.IsPressed())
        {
            playerMovementSc.interactPropmt = false;
        }
    }
    private void OnEnable()
    {
        playerActionsAsset.Player.Enable();
        playerActionsAsset.Player.Interact.started += DoVentInteract;
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Disable();
        playerActionsAsset.Player.Interact.started -= DoVentInteract;
    }

    private void DoVentInteract(InputAction.CallbackContext context)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (playerMovementSc.isHuman && playerActionsAsset.Player.Interact.IsPressed())
        {
            playerMovementSc.interactPropmt = false;
            this.gameObject.SetActive(false);
        }
    }
}
