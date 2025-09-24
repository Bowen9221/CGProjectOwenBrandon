using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardUse : MonoBehaviour
{
    public GameObject door;

    public AudioSource keyCardSound;
    public AudioSource errorSound;

    private void OnTriggerStay(Collider other)
    {
        //So only human can use the card
        if (other.CompareTag("Human") && Input.GetKeyDown(KeyCode.E))
        {
            //If it = true
            if (KeyPickup.hasKey)
            {
                //gets rid of yellow cube 
                Destroy(gameObject);

                //Break door 
                Destroy(door);

                keyCardSound.Play();

                Debug.Log("Door Unlocked");
            }

            else
            {
                errorSound.Play();
                Debug.Log("You need the key card");
            }
        }
    }
}
