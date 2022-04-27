using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualFXSystem;

public class FootSteps : MonoBehaviour
{
    public Transform[] feet;
    public VisualFX vfx;

    public void Step(int foot)
    {
        vfx.Begin(feet[foot - 1]);
    }
}
