using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script contains the movement connecting to the player input system in unity
// as well as the switch between the 2 companions,
// ground check, player rotation with the camera, 
// and the ray cast that checks if anything is in the way between compainion and player
// ^ which allows for player to switch bettween the two


public class ThirdPersonMovement : MonoBehaviour
{
    //companion camera
    [SerializeField]
    private GameObject companionCamera;

    //input vars
    private ThirdPersonActions playerActionsAsset;
    private InputAction move;

    //switch Character Bool
    private bool isHuman = true;

    //Companion Obj
    [SerializeField]
    private GameObject companion;

    //Companion Vars
    private Rigidbody cRb;
    [SerializeField]
    private float CompanionMovementForce = 2f;
    [SerializeField]
    private float CompanionMaxSpeed = 8f;

    //Human movement vars
    private Rigidbody rb;
    [SerializeField]
    private float HumanMovementForce = 1f;
    [SerializeField]
    private float HumanMaxSpeed = 5f;
    private Vector3 forceDir = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float playerRotationSpeed = 1f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

        cRb = companion.GetComponent<Rigidbody>();
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActions();
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.Jump.started += DoJump;
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
        playerActionsAsset.Player.PlayerSwitch.started += DoPlayerSwitch;

    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Jump.started -= DoJump;
        playerActionsAsset.Player.Disable();
        playerActionsAsset.Player.PlayerSwitch.started -= DoPlayerSwitch;
    }

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
                cRb.velocity -= Vector3.down * Physics.gravity.y *Time.fixedDeltaTime;

            Vector3 horizontalVelocity = cRb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > CompanionMaxSpeed * CompanionMaxSpeed)
                cRb.velocity = horizontalVelocity.normalized * CompanionMaxSpeed + Vector3.up * cRb.velocity.y;

        }

        LookAt();
    }

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

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext context)
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

    //player switch
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

            if (hit.collider.gameObject.name == companion.gameObject.name && distance <= 10)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    
}
