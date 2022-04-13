using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    public void Toggle ()
    {
        if (gameObject == null)
            return;

        gameObject.SetActive(!gameObject.activeSelf);
    }
}
