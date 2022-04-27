using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class BetterUnity
{
    public static T GetOrAddComponent<T>(this GameObject current) where T : Component
    {
        Component comp = current.GetComponent<T>();

        return (comp ? comp : current.AddComponent<T>()) as T;
    }
}
