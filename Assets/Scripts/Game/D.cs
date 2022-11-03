using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D : MonoBehaviour
{
    public static bool Enabled = false;

    public static void Log(object o, GameObject g = null)
    {
        if (Enabled)
        {
            if (g == null)
            {
                Debug.Log(o);
            }
            else
            {
                Debug.Log(o, g);
            }

        }
    }
}
