using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

/* This script contains the movement connecting to the player input system in unity
 as well as the switch between the 2 companions,
 ground check, player rotation with the camera, 
 and the ray cast that checks if anything is in the way between compainion and player
 ^ which allows for player to switch bettween the two
 contains the vaulting system
 and the prompt system in addition with the "PopUps" script

*/

public class ThirdPersonMovement : MonoBehaviour
{
    
    


    [SerializeField]
    private float changePlayerMaxDistance = 10f;



    [Header("Companion Movement Settings")]
    [SerializeField]
    private float CompanionMovementForce = 2f;
    [SerializeField]
    private float CompanionMaxSpeed = 8f;
    //Companion Obj
    [SerializeField]
    private GameObject companion;
    //companion camera
    [SerializeField]
    private GameObject companionCamera;

    [Header("Human Movement Settings")]
    [SerializeField]
    private float HumanMovementForce = 1f;
    [SerializeField]
    private float HumanMaxSpeed = 5f;
    private Vector3 forceDir = Vector3.zero;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float playerRotationSpeed = 1f;

    //input vars
    private ThirdPersonActions playerActionsAsset;
    private InputAction move;

    private Rigidbody cRb;
    private Rigidbody rb;

    //switch Character Bool
    [HideInInspector]
    public bool isHuman = true;

    [Header("Mantle Settings")]

    public float playerHeight = 2f;
    public float playerRadius = 0.5f;
    public float maxVaultHeight = 1.0f;
    public LayerMask vaultLayer;

    //popUp variables
    [HideInInspector] public bool ableVault;
    [HideInInspector] public bool interactPropmt;
    [HideInInspector] public bool canIntDia;

    [HideInInspector] public string ActiveController = "keyboard";


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cRb = companion.GetComponent<Rigidbody>();
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActions();
    }

    void Start()
    {
        
    }

    private void Update()
    {
        //button popup checks //kept in update cuz update updates the code every frame
        CheckVaultable();




    }



    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        
        if (context.control.device is Keyboard || context.control.device is Mouse)
        {
            Debug.Log("Input from KbM");

            ActiveController = "keyboard";
        }
        else if (context.control.device is Gamepad gamepad)
        {
            string productName = gamepad.device.description.product?.ToLower();

            if (!string.IsNullOrEmpty(productName))
            {
                if (productName.Contains("xbox"))
                {
                    Debug.Log("Input from xbox controller");
                    ActiveController = "xbox";

                }
                else if (productName.Contains("dualshock") || productName.Contains("dual sense") || productName.Contains("playstation") || productName.Contains("ps"))
                {
                    Debug.Log("Input from Playstation Controller");
                    ActiveController = "ps";
                }
                else
                {
                    Debug.Log("Input from other Gamepad:" + productName);

                    ActiveController = "xbox";
                }
            }
            else
            {
                Debug.Log("Input from Generic Gamepad");

                ActiveController = "xbox";
            }
        }
    }






    

    private void OnEnable()
    {
        //playerActionsAsset.Player.Jump.started += DoJump;

        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
        playerActionsAsset.Player.PlayerSwitch.started += DoPlayerSwitch;

        playerActionsAsset.Player.Vault.started += DoVault;




        //actionperformed check //KEEP AT BOTTOM OF ENABLE//
        move.Enable();
        move.performed += OnActionPerformed;
        //KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//KEEP AT BOTTOM OF ENABLE//
    }

    private void OnDisable()
    {
        //playerActionsAsset.Player.Jump.started -= DoJump;

        playerActionsAsset.Player.Disable();
        playerActionsAsset.Player.PlayerSwitch.started -= DoPlayerSwitch;

        playerActionsAsset.Player.Vault.started -= DoVault;




        //actionperformed check//KEEP AT BOTTOM//
        move.Enable();
        move.performed += OnActionPerformed;
        //KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//KEEP AT BOTTOM OF DISABLE//
    }



    //using fixed update works better for physics and movements using physics
    private void FixedUpdate()
    {

        if (isHuman)
        {
            forceDir += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * HumanMovementForce;
            forceDir += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * HumanMovementForce;

            rb.AddForce(forceDir, ForceMode.Impulse);
            forceDir = Vector3.zero;

            if (rb.velocity.y < 0f)
                rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

            Vector3 horizontalVelocity = rb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > HumanMaxSpeed * HumanMaxSpeed)
                rb.velocity = horizontalVelocity.normalized * HumanMaxSpeed + Vector3.up * rb.velocity.y;


        }
        else if (!isHuman)
        {
            forceDir += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * CompanionMovementForce;
            forceDir += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * CompanionMovementForce;

            cRb.AddForce(forceDir, ForceMode.Impulse);
            forceDir = Vector3.zero;

            if (cRb.velocity.y < 0f)
                cRb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

            Vector3 horizontalVelocity = cRb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > CompanionMaxSpeed * CompanionMaxSpeed)
                cRb.velocity = horizontalVelocity.normalized * CompanionMaxSpeed + Vector3.up * cRb.velocity.y;

        }

        LookAt();
    }




    //this makes the player turn and look the direction it moves
    private void LookAt()
    {
        if (isHuman)
        {
            Vector3 direction = rb.velocity;
            direction.y = 0f;

            if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            {
                //snappy rotation
                //this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up); 

                //less snappy rotation
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
            }
            else
                rb.angularVelocity = Vector3.zero;
        }

        else if (!isHuman)
        {
            Vector3 direction = cRb.velocity;
            direction.y = 0f;

            if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            {
                //snappy rotation
                //this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up); 

                //less snappy rotation
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                cRb.rotation = Quaternion.Slerp(cRb.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
            }
            else
                cRb.angularVelocity = Vector3.zero;
        }



    }
    

    //this gets the rotation of camera when moving character to turn it 
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    // ^^^
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }



    // this shoots raycasts to find if the vaulting platform is vaultable
    private void DoVault(InputAction.CallbackContext context)
    {

        if (isHuman)
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out var firstHit, 1f, vaultLayer))
            {
                Debug.Log("vaultible INfrount");
                if (Physics.Raycast(firstHit.point + (this.transform.forward * playerRadius) + (Vector3.up * maxVaultHeight * playerHeight), Vector3.down, out var secondHit, playerHeight))
                {
                    Debug.Log("found Place to land");
                    StartCoroutine(LerpVault(secondHit.point, 0.5f));
                }
            }
        }
    }




    //this starts the moving character
    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        OnDisable();
        float time = 0;
        Vector3 startPosition = transform.position;
        targetPosition += Vector3.up * playerHeight / 2;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        OnEnable();
    }


    /* jump method
     * 
     * private void DoJump(InputAction.CallbackContext context)
     {
         if (IsGrounded())
         {
             if (isHuman)
             {
                // forceDir += Vector3.up * HumanJumpForce;
             }
             else if (!isHuman)
             {
                // forceDir += Vector3.up * CompanionJumpForce;
             }


         }


     }
    */ //JUMP METHOD


    //Checks if the player is grounded if jump is turned on
    private bool IsGrounded()
    {
        if (isHuman)
        {
            Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.4f))
                return true;
            else
                return false;
        }
        else if (!isHuman)
        {
            Ray ray = new Ray(companion.transform.position + Vector3.up * 0.25f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
                return true;
            else
                return false;
        }

        return false;
    }




    //player switch / Companion switch
    private void DoPlayerSwitch(InputAction.CallbackContext context)
    {
        if (isHuman && CheckDistance())
        {
            companionCamera.SetActive(true);

            isHuman = false;

            Debug.Log("Player Switch activated");
        }
        else if (!isHuman && CheckDistance())
        {
            companionCamera.SetActive(false);

            isHuman = true;

            Debug.Log("Player Switch activated");
        }


    }


    //checks distance between player and coompanion and if there is anything inbetween them 
    private bool CheckDistance()
    {
        Vector3 origin = this.transform.position;
        Vector3 target = companion.transform.position;

        Vector3 direction = (target - origin).normalized;
        float distance = Vector3.Distance(origin, target);

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            // Debug.Log($"Raycast hit: {hit.collider.gameObject.name} at {hit.point}");

            if (hit.collider.gameObject.name == companion.gameObject.name && distance <= changePlayerMaxDistance)
            {
                return true;
            }
            return false;
        }
        return false;
    }








    //POP UP STUFF////////////////////////////////////////////////////


    //This Checks if there is a vaultable object infront of the player
    private void CheckVaultable()
    {
        if (isHuman)
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out var firstHit, 2f, vaultLayer))
            {
                ableVault = true;
            }
            else
            {
                ableVault = false;
            }

        }


    }

    
    //this is temp, code a interact prompt better
    private void OnTriggerStay(Collider other)
    {
        if (isHuman && other.CompareTag("vent"))
        {
            interactPropmt = true;
        }

        if (isHuman && other.CompareTag("dialouge"))
        {
            canIntDia = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactPropmt)
        {
            interactPropmt = false;
        }

        if (canIntDia)
        {
            canIntDia = false;
        }
    }


}
