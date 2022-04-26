using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public Material matA;
    public Material matB;
    bool matState = true;
    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    public void Swap()
    {
        matState = !matState;
        meshRenderer.material = matState ? matA : matB;
    }
}
