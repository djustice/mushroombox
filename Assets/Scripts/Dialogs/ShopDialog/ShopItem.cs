using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public GameObject purchaseDialog;
    public GameObject shopDialog;
    public GameObject maskTop;
    public Image sporeImage;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI item;
    public Animator anim;


    public string itemID;

    public void Start()
    {
        //item = purchaseDialog.GetComponentInChildren<TextMeshProUGUI>();

        //if (Game.coinCount >= int.Parse(costText.text))
        //{
        //    costText.color = new Color(0f, 0.5f, 0f, 1f);
        //}
        //else
        //{
        //    costText.color = new Color(0.6f, 0f, 0f, 1f);
        //}
    }

    private void OnMouseDown()
    {
        //transform.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseUp()
    {
        //transform.GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
        Animator purchaseAnimation = purchaseDialog.GetComponent<Animator>();

        anim = shopDialog.GetComponent<Animator>();
        anim.SetInteger("state", 2);
	    //
        if (itemID.StartsWith("Spores"))
            item.text = "Spores";
	    //
        if (itemID == "Spores.10")
        {
            if (Game.coinCount >= 20)
            {
                purchaseAnimation.SetInteger("state", 1);
                sporeImage.gameObject.SetActive(true);
                costText.text = "20";
                quantityText.text = "10x";
            }
        } else if (itemID == "Spores.30")
        {
            if (Game.coinCount >= 50)
            {
                purchaseAnimation.SetInteger("state", 1);
                sporeImage.gameObject.SetActive(true);
                costText.text = "50";
                quantityText.text = "30x";
            }
        }
        else if (itemID == "Spores.80")
        {
            if (Game.coinCount >= 100)
            {
                purchaseAnimation.SetInteger("state", 1);
                sporeImage.gameObject.SetActive(true);
                costText.text = "100";
                quantityText.text = "80x";
            }
        }
        else if (itemID == "Spores.500")
        {
            if (Game.coinCount >= 500)
            {
                purchaseAnimation.SetInteger("state", 1);
                sporeImage.gameObject.SetActive(true);
                costText.text = "500";
                quantityText.text = "500x";
            }
        }
    }
}
