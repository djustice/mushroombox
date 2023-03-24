using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsDialogHandler : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnSettingsMouseUp()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetInteger("state", 1);
    }

    public void OnCloseMouseUp()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetInteger("state", 2);
    }

    public void OnRestartGameMouseUp()
    {
        SceneManager.LoadScene("TitleScene");
//        gameObject.SetActive(false);
    }

    public void OnTestButtonUp()
    {
        Game.coinCount = Game.coinCount + 1000;
        Game.sporeCount = Game.sporeCount + 50;
    }
}
