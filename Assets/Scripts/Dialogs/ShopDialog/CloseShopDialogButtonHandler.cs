using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseShopDialogButtonHandler : MonoBehaviour
{
    public GameObject shopDialog;
    public GameObject maskTop;
    public GameObject maskBottom;
    public Animator anim;
    public void OnCloseButtonClicked()
    {
        Debug.Log("mask " + maskTop.name);
        maskTop.SetActive(false);
        Debug.Log("mask " + maskBottom.name);
        maskBottom.SetActive(false);

        anim = shopDialog.GetComponent<Animator>();
        anim.SetInteger("state", 2);
    }
}
