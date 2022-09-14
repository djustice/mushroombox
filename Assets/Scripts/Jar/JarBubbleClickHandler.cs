using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarBubbleClickHandler : MonoBehaviour
{
    public Jar jar;

    private void OnMouseUp()
    {
        if (jar.bubble.sprite != jar.bubbleSprites[2])
        {
            jar.OnMouseUp();
        }
    }
}
