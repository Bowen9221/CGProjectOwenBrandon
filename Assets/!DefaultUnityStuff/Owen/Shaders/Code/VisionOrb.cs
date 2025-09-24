using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionOrb : MonoBehaviour
{
    public Material visionLimiter;
    public Transform fox;
    public float visionRadius;
    public float feather;
    

    public CinemachineBrain brain;
    public CinemachineFreeLook foxVCam;
    public CinemachineFreeLook humanVCam;

    private void Update()
    {
        visionLimiter.SetVector("_FoxPos", fox.position);
        visionLimiter.SetFloat("_VisionRadius", visionRadius);
        visionLimiter.SetFloat("Feather", feather);

        ICinemachineCamera activeCam = brain.ActiveVirtualCamera;
        if (activeCam == foxVCam)
        {
            visionLimiter.SetFloat("_IsFox", 1f);
        }
        else if (activeCam == humanVCam)
        {
            visionLimiter.SetFloat("_IsFox", 0f);
        }
        Debug.Log(visionLimiter.GetFloat("_IsFox"));
    }
}
