using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShaderSwap : MonoBehaviour
{
    public List<GameObject> models = new List<GameObject>();
    public List<Material> materials = new List<Material>();

    int currentIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (materials.Count == 0 || models.Count == 0)
            return;

        foreach (var obj in models)
        {
            var rend = obj.GetComponentInChildren<Renderer>();
            if (rend != null)
                rend.material = materials[currentIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex = (currentIndex + 1) % materials.Count;

            foreach (var obj in models)
            {
                var rend = obj.GetComponentInChildren<Renderer>();
                if (rend != null)
                    rend.material = materials[currentIndex];
            }
        }
    }
}
