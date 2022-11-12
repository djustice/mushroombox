using UnityEngine;
using UnityEngine.UI;

using MushroomBox.Debug;

public class WalkableArea : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnWalkableAreaMouseUp()
    {
        if (Game.player.walkingJars == true || Game.player.walkingBoxes == true)
            return;

        if (Game.player.customerWaiting == true)
            return;

        if (Game.player.isMoving == true)
            return;

        Vector2 mousePos = Input.mousePosition;
        this.D("Walkable Area: " + Input.mousePosition);

        if (Input.touchCount == 1)
            if (Input.GetTouch(0).phase != TouchPhase.Moved)
                Game.player.WalkToThenStop(mousePos);
    }
}
