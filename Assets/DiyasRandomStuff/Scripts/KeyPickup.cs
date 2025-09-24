using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool hasKey = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pet") && Input.GetKeyDown(KeyCode.E))
        {
            hasKey = true;

            Debug.Log("You have retrieved the keycard");
            Destroy(gameObject);
        }
    }
}
