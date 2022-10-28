using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject walkableArea;
    public Image Vignette;
    public TextMeshProUGUI[] Text;

    void Start()
    {

    }

    public void OnMouseClick()
    {
        if (Text[0].IsActive() == true) {
            Vignette.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector2 pos = new Vector2(Game.boxes[0].transform.position.x, Game.boxes[0].transform.position.y - 100);
            Vignette.transform.position = pos;
            Text[0].gameObject.SetActive(false);
            Text[1].gameObject.SetActive(true);
            Vignette.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else if (Text[1].IsActive() == true)
        {
            walkableArea.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void StartTutorial()
    {
        gameObject.SetActive(true);
    }
}
