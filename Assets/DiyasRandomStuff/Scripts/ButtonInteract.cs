using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    //will be green barrier
    public GameObject blockedArea;

    public AudioSource btnSound;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Human") && Input.GetKeyDown(KeyCode.E))
        {
            if (blockedArea != null) Destroy(blockedArea);

            //Play button sound effect
            btnSound.Play();

            Debug.Log("Green barricade down");

            //I wanna add a particle effect here too cuz y not
            Destroy(gameObject);
        }
    }
}
