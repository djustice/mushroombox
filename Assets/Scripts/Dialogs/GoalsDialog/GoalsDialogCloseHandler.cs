using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalsDialogCloseHandler : MonoBehaviour
{
	public GameObject goalsDialog;
    public GameObject maskTop;
	public GameObject maskBottom;
	public GoalsButtonHandler goalsButton;
	public Animator anim;
    
    public void OnCloseButtonClicked()
    {
	    Debug.Log("GoalsDialog : OnCloseButtonClicked");

	    anim = goalsDialog.GetComponent<Animator>();
	    anim.SetBool("Idle", false);
	    anim.SetBool("Extend", false);
	    anim.SetBool("Retract", true);
    }
}
