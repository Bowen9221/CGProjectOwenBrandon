using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSwitching : MonoBehaviour
{
    ThirdPersonMovement player;
    public Material visionMat;


    void Start()
    {
        player = FindAnyObjectByType<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isHuman)
            visionMat.SetFloat("_isFox", 1f);
        else
            visionMat.SetFloat("_isFox", 0f);
    }
}
