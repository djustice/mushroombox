using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MushroomBox.Debug;

public class GoalsDialogCloseHandler : MonoBehaviour
{
	public GameObject goalsDialog;
    public GameObject maskTop;
	public GameObject maskBottom;
	public GameObject blurMask1;
	public GameObject blurMask2;
	public GoalsButtonHandler goalsButton;
	public Animator anim;
    
    public void OnCloseButtonClicked()
    {
	    this.D("GoalsDialog : OnCloseButtonClicked");

	    anim = goalsDialog.GetComponent<Animator>();
	    anim.SetBool("Idle", false);
	    anim.SetBool("Extend", false);
	    anim.SetBool("Retract", true);

		anim = blurMask1.GetComponent<Animator>();
		anim.SetTrigger("FadeOut");
        anim = blurMask2.GetComponent<Animator>();
        anim.SetTrigger("FadeOut");
    }
}
