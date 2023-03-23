using UnityEngine;
using UnityEngine.UI;

using MushroomBox.Debug;

public class WalkableArea : MonoBehaviour
{
    void Start()
    {
        Physics2D.queriesHitTriggers = true;
    }

    public void OnMouseUp()
    {
        this.D("WalkableArea,OnMouseUp");
        if (Game.player.walkingJars == true || Game.player.walkingBoxes == true)
            return;

        if (Game.player.customerWaiting == true)
            return;

        if (Game.player.isMoving == true)
            return;

        Vector2 mousePos = Input.mousePosition;

        if (Input.touchCount == 1)
            if (Input.GetTouch(0).phase != TouchPhase.Moved)
                Game.player.WalkToThenStop(mousePos);
    }

    //public void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward * 1000);

    //        if (hit2D.collider != null)
    //        {
    //            this.D("Walkable Area: " + Input.mousePosition);

    //            if (Game.player.walkingJars == true || Game.player.walkingBoxes == true)
    //                return;

    //            if (Game.player.customerWaiting == true)
    //                return;

    //            if (Game.player.isMoving == true)
    //                return;

    //            Vector2 mousePos = Input.mousePosition;

    //            if (Input.touchCount == 1)
    //                if (Input.GetTouch(0).phase != TouchPhase.Moved)
    //                    Game.player.WalkToThenStop(mousePos);
    //        }
    //    }
    //}
}
