using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MushroomBox.Debug;

public class ShopButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI[] costTexts;
    
    public Animator buttonAnim;
    public Animator dialogAnim;

    public bool dialogOpen = false;

    public void OnMouseUp()
    {
        this.D("OnMouseUp");
        StartCoroutine("Toggle");
    }

    public void OnMaskClicked()
    {
        this.D("Mask Clicked");
        StartCoroutine("Toggle");
    }

    IEnumerator Toggle()
    {
        yield return null;

        if (dialogOpen == false)
        {
            foreach (TextMeshProUGUI costText in costTexts)
            {
                if (Game.coinCount >= int.Parse(costText.text))
                {
                    costText.color = new Color(0f, 0.8f, 0f, 1f);
                }
                else
                {
                    costText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }

            GetComponent<Button>().interactable = false;
            dialogOpen = true;
            this.D("buttonAnim, extend");
            buttonAnim.SetBool("Idle", false);
            buttonAnim.SetBool("Extend", true);
            buttonAnim.SetBool("Retract", false);
            yield return new WaitForSeconds(1);
            this.D("buttonAnim, extended");
            this.D("dialogAnim, extend");
            dialogAnim.SetBool("Idle", false);
            dialogAnim.SetBool("Extend", true);
            dialogAnim.SetBool("Retract", false);
            yield return new WaitForSeconds(1);
            this.D("dialogAnim, extended");
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
            this.D("dialogAnim, retract");
            dialogAnim.SetBool("Idle", false);
            dialogAnim.SetBool("Extend", false);
            dialogAnim.SetBool("Retract", true);
            yield return new WaitForSeconds(1);
            this.D("dialogAnim, retracted");
            this.D("buttonAnim, retract");
            buttonAnim.SetBool("Idle", false);
            buttonAnim.SetBool("Extend", false);
            buttonAnim.SetBool("Retract", true);
            yield return new WaitForSeconds(1);
            this.D("buttonAnim, retracted");
            dialogOpen = false;
            GetComponent<Button>().interactable = true;
        }
    }
}
