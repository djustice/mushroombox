using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MushroomBox.Debug;

public class GoalsButtonHandler : MonoBehaviour
{
    public GameObject goalsDialog;
    public GameObject[] masks;

    public Animator anim;

    public void OnMouseUp()
    {
        this.D("goal button");

        //foreach (GameObject mask in masks)
        //{
        //    mask.SetActive(true);
        //}

        //if (anim.GetBool("Idle") == true || anim.GetBool("Retract") == true) 
        //{
	    //    anim.SetBool("Idle", false);
	    //    anim.SetBool("Retract", false);
        //    anim.SetBool("Extend", true);
	    //    Game.shopButton.gameObject.SetActive(false);
	    //    Game.settingsButton.gameObject.SetActive(false);
        //    return;
        //}

        //if (anim.GetBool("Extend") == true)
        //{
        //    anim.SetBool("Idle", false);
        //    anim.SetBool("Extend", false);
	    //    Animator dialogAnim = goalsDialog.GetComponent<Animator>();
	    //    dialogAnim.SetBool("Idle", false);
	    //    dialogAnim.SetBool("Extend", false);
	    //    dialogAnim.SetBool("Retract", true);
        //    return;
        //}
    }

    public void ExtendDialog()
	{
		//D.Log("GoalsButton : ExtendDialog");
		//masks[0].SetActive(true);
		//masks[1].SetActive(true);
        //Animator dialogAnim = goalsDialog.GetComponent<Animator>();
        //dialogAnim.SetBool("Idle", false);
        //dialogAnim.SetBool("Retract", false);
        //dialogAnim.SetBool("Extend", true);
    }

	public void RetractButton()
	{
		//D.Log("GoalsButton : RetractDialog");
		//anim.SetBool("Idle", false);
		//anim.SetBool("Extend", false);
		//anim.SetBool("Retract", true);
		
		//Game.shopButton.gameObject.SetActive(true);
		//Game.settingsButton.gameObject.SetActive(true);
    }
}
