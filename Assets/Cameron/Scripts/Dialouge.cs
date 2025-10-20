using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialouge : MonoBehaviour
{
    private ThirdPersonActions playerActionsAsset;
    private ThirdPersonMovement playerMovementSc;

    public TMP_Text diaTxt;

    public string npc_Text;



    private void Awake()
    {
        playerActionsAsset = new ThirdPersonActions();
    }


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
        playerActionsAsset.Player.Interact.started += DoDiaInt;
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Disable();
        playerActionsAsset.Player.Interact.started -= DoDiaInt;
    }

    private void DoDiaInt(InputAction.CallbackContext context)
    {

    }


    private void OnTriggerStay(Collider other)
    {


        if (playerMovementSc.isHuman && playerActionsAsset.Player.Interact.IsPressed())
        {
            playerMovementSc.canIntDia = false;
            diaTxt.text = npc_Text;
        }
    }
}
