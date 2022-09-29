using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtonHandler : MonoBehaviour
{
    public GameObject newGameDialog;
    public Animator fadeAnim;

    public void OnContinueClicked()
    {
        string path = Application.persistentDataPath + "/save.txt";
        if (File.Exists(path))
        {
            fadeAnim.SetTrigger("FadeOut");
        }
    }

    public void OnNewGameClicked()
    {
        string path = Application.persistentDataPath + "/save.txt";
        if (File.Exists(path))
        {
            newGameDialog.SetActive(true);
        }
        else
        {
            fadeAnim.SetTrigger("FadeOut");
        }
    }

    public void LoadL1() {
        SceneManager.LoadScene("L1");
    }
}
