using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPopups : MonoBehaviour
{
    private ThirdPersonMovement playerMovementSc;



    //Vault Text and Images
    [Header("Vaulting Prompt")]
    [SerializeField]
    private GameObject vaultText;
    [SerializeField]
    private GameObject spaceBarVault;
    [SerializeField]
    private GameObject psX;
    [SerializeField]
    private GameObject xboxA;

    [Header("Interact Prompt")]
    [SerializeField]
    private GameObject interactText;
    [SerializeField]
    private GameObject kbmE;
    [SerializeField]
    private GameObject psSquare;
    [SerializeField]
    private GameObject xboxX;
    // Start is called before the first frame update
    void Start()
    {
        playerMovementSc = FindAnyObjectByType<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Updates vault popups
        VaultPopUps();


        //updates Interact 
        InteractPopUp();

    }


    public void VaultPopUps()
    {
        if (playerMovementSc.ableVault)
        {
            vaultText.SetActive(true);
            if (playerMovementSc.ActiveController == "keyboard")
            {
                psX.SetActive(false);
                xboxA.SetActive(false);
                spaceBarVault.SetActive(true);
            }
            else if (playerMovementSc.ActiveController == "xbox")
            {
                psX.SetActive(false);
                spaceBarVault.SetActive(false);
                xboxA.SetActive(true);
            }
            else if (playerMovementSc.ActiveController == "ps")
            {
                xboxA.SetActive(false);
                spaceBarVault.SetActive(false);
                psX.SetActive(true);
            }
        }
        else if (!playerMovementSc.ableVault)
        {
            vaultText.SetActive(false);
            psX.SetActive(false);   
            xboxA.SetActive(false);
            spaceBarVault.SetActive(false);
        }
    }


    public void InteractPopUp()
    {
        if (playerMovementSc.interactPropmt)
        {
            interactText.SetActive(true);

            if (playerMovementSc.ActiveController == "keyboard")
            {
                psSquare.SetActive(false);
                xboxX.SetActive(false);
                kbmE.SetActive(true);
            }
            else if (playerMovementSc.ActiveController == "xbox")
            {
                psSquare.SetActive(false);
                kbmE.SetActive(false);
                xboxX.SetActive(true);
            }
            else if (playerMovementSc.ActiveController == "ps")
            {
                xboxX.SetActive(false);
                kbmE.SetActive(false);
                psSquare.SetActive(true);
            }
        }
        else if (!playerMovementSc.ableVault)
        {
            interactText.SetActive(false);
            psSquare.SetActive(false);
            xboxX.SetActive(false);
            kbmE.SetActive(false);
        }
    }
}
