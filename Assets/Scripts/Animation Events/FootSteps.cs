using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public Transform[] feet;
    public GameObject prefab;

    public void Step(int foot)
    {
        GameObject obj = Instantiate(prefab, feet[foot - 1].position, Quaternion.identity);

        Destroy(obj, 5);
    }
}
