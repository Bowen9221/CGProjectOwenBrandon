using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOT USING THIS SCRIPT

public class DoorUnlock : MonoBehaviour
{
    

    private void OnTriggerStay(Collider other)
    {
        //Just in case somehow the pet can reach the button
        if (other.CompareTag("Human") && Input.GetKeyDown(KeyCode.E))
        {
            //If it = true
            if (KeyPickup.hasKey)
            {
                

                //maybe add particle effect here
                Destroy(gameObject);
            }

            else
            {
                //Add sound here or text saying can't open without key
                
            }
        }
    }
}
