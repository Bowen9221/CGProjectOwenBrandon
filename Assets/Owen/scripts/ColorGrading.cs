using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorGrading : MonoBehaviour
{
    public Material LUTMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("OnRenderImage Called");

        if (LUTMaterial != null)
            Graphics.Blit(source, destination, LUTMaterial);
        else
            Graphics.Blit(source, destination)
;    }
}
