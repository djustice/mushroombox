using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Placeholder : MonoBehaviour
{
    public GameObject purchaseDialog;
    public int item;
    public Image jarImage;
    public Image boxImage;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI quantityText;

    public void OnMouseUp()
    {
        if (item == 1)
        {
            purchaseDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Jar";
            jarImage.gameObject.SetActive(true);
            costText.text = "100";
            quantityText.text = "1x";
        }
        else if (item == 2)
        {
            purchaseDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Box";
            boxImage.gameObject.SetActive(true);
            costText.text = "300";
            quantityText.text = "1x";
        }

        if (Game.coinCount >= int.Parse(costText.text))
        {
            costText.color = new Color(0f, 1f, 0f, 1f);
        }
        else
        {
            costText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        purchaseDialog.GetComponent<Animator>().SetInteger("state", 1);
    }
}
