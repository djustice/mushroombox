using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanArrow : MonoBehaviour
{
    public GameObject[] arrowButtons;

    public Animator cameraAnimator;
    public enum ArrowDirection { Left, Right };
    public ArrowDirection direction;

    public void OnMouseUp()
    {
        if (direction == ArrowDirection.Left) {
            arrowButtons[0].SetActive(false);
            arrowButtons[1].SetActive(true);
            cameraAnimator.SetTrigger("PanLeft");
        }
        else
        {
            arrowButtons[0].SetActive(true);
            arrowButtons[1].SetActive(false);
            cameraAnimator.SetTrigger("PanRight");
        }
    }
}
