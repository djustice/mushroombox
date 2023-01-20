using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MushroomBox.Debug;

public class SettingsButtonHandler : MonoBehaviour
{
	public Animator dialogAnim;
	public Animator buttonAnim;

	public bool dialogOpen = false;

    public void OnMouseUp()
	{
		this.D("OnMouseUp");
		StartCoroutine("Toggle");
    }

	public void OnMaskClicked()
	{
        this.D("OnMaskClicked");
        StartCoroutine("Toggle");
	}
	
	IEnumerator Toggle() 
	{
		yield return null;

        if (dialogOpen == false)
        {
            GetComponent<Button>().interactable = false;
            dialogOpen = true;

            Game.shopButton.SetActive(false);
            Game.goalsButton.SetActive(false);

            buttonAnim.SetBool("Idle", false);
            buttonAnim.SetBool("Extend", true);
            buttonAnim.SetBool("Retract", false);
            yield return new WaitForSeconds(1);
            dialogAnim.SetBool("Idle", false);
            dialogAnim.SetBool("Extend", true);
            dialogAnim.SetBool("Retract", false);
            yield return new WaitForSeconds(1);
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;

            dialogAnim.SetBool("Idle", false);
            dialogAnim.SetBool("Extend", false);
            dialogAnim.SetBool("Retract", true);
            yield return new WaitForSeconds(1);
            buttonAnim.SetBool("Idle", false);
            buttonAnim.SetBool("Extend", false);
            buttonAnim.SetBool("Retract", true);
            yield return new WaitForSeconds(0.6f);

            Game.shopButton.SetActive(true);
            Game.goalsButton.SetActive(true);

            dialogOpen = false;
            GetComponent<Button>().interactable = true;
        }
	}
}
