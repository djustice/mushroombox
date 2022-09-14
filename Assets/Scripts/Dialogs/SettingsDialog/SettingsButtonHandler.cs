using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsButtonHandler : MonoBehaviour
{
	public GameObject settingsDialog;
	public GameObject settingsButton;
	
    public TextMeshProUGUI[] costTexts;

    public void OnMouseUp()
	{
		Game.shopButton.SetActive(false);
		Game.goalsButton.SetActive(false);
	    Animator anim = GetComponent<Animator>();
	    anim.SetBool("Idle", false);
	    anim.SetBool("Retract", false);
		anim.SetBool("Extend", true);
		Game.shopButton.SetActive(false);
		Game.goalsButton.SetActive(false);
    }
    
	public void ShowSettingsDialog() 
	{
		Animator anim = settingsDialog.GetComponent<Animator>();
		anim.SetInteger("state", 1);
	}
	
	public void RetractButton() 
	{
		Animator anim = settingsButton.GetComponent<Animator>();
		anim.SetBool("Idle", false);
		anim.SetBool("Extend", false);
		anim.SetBool("Retract", true);
		StartCoroutine("Wait");
	}
	
	IEnumerator Wait() 
	{
		yield return new WaitForSeconds(0.5f);
		Game.shopButton.SetActive(true);
		Game.goalsButton.SetActive(true);
	}
}
