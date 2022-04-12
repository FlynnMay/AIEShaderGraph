using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static void DoAction(Action action)
    {
        Debug.Log($"Doing {action.actionName}");
    }
}
