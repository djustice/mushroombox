using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonHandler : MonoBehaviour
{
    public GameObject shopDialog;
    public GameObject[] masks;
    public TextMeshProUGUI[] costTexts;
    public Animator anim;

    public void OnMouseUp()
    {
        foreach (GameObject mask in masks)
        {
            mask.SetActive(true);
        }

        anim = shopDialog.GetComponent<Animator>();
        anim.SetInteger("state", 1);
    }
}
