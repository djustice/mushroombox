using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBubbleClickHandler : MonoBehaviour
{
    public Box box;

    private void OnMouseUp()
    {
        if (box.bubble.sprite == box.bubbleSprites[1])
        {
            box.OnMouseUp();
        }
    }
}
