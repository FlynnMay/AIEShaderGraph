using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionList : MonoBehaviour
{
    public List<Action> actions = new List<Action>();

    public UnityEvent onChanged;

    [ContextMenu("Delete First")]
    void DeleteFirst()
    {
        actions.RemoveAt(0);
        onChanged.Invoke();
    }
}
