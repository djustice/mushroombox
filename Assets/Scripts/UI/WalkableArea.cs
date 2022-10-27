using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkableArea : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnWalkableAreaClick()
    {
        if (Game.player.customerWaiting == true)
            return;

        if (Game.player.isMoving == true)
            return;

        Vector2 mousePos = Input.mousePosition;
        Debug.Log("Walkable Area: " + Input.mousePosition);

        Game.player.WalkToThenStop(mousePos);

        // float distX;
        // float distY;

        // if (mousePos.x >= Game.player.transform.position.x)
        // {
        //     distX = mousePos.x - Game.player.transform.position.x;
        // } else {
        //     distX = Game.player.transform.position.x - mousePos.x;
        // }

        // if (mousePos.y >= Game.player.transform.position.y)
        // {
        //     distY = mousePos.y - Game.player.transform.position.y;
        // } else {
        //     distY = Game.player.transform.position.y - mousePos.y;
        // }

        // if (distX > distY) {
        //     if (mousePos.x >= Game.player.transform.position.x)
        //     {
        //         Game.player.Walk(Direction.Right, mousePos);
        //     }
        //     else
        //     {
        //         Game.player.Walk(Direction.Left, mousePos);
        //     }
        // } else {
        //     if (mousePos.y >= Game.player.transform.position.y)
        //     {
        //         Game.player.Walk(Direction.Up, mousePos);
        //     }
        //     else
        //     {
        //         Game.player.Walk(Direction.Down, mousePos);
        //     }
        // }

    }
}
