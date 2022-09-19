using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDialog : MonoBehaviour
{
    public bool isActive;
    public bool eventFlag = false;
    private TextDisplayEffect _effect;
    private Animator _animator;

    public void Start()
    {
        _effect = GetComponent<TextDisplayEffect>();
        _animator = GetComponent<Animator>();
    }

    public void ShowDialog()
    {
        isActive = true;
        _animator.SetBool("Show", true);
    }

    public void StartDialog()
    {
        _effect.PlayPage();
    }

    public void CloseDialog()
    {
	    _animator.SetBool("Show", false);
	    Game.shopButton.SetActive(true);
	    Game.goalsButton.SetActive(true);
	    Game.settingsButton.SetActive(true);
        isActive = false;
    }

    public void OnMouseUp()
    {
        if (_effect.isIdle == true)
        {
            _effect.PlayPage();
        }
    }
}
