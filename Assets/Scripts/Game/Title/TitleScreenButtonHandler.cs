using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtonHandler : MonoBehaviour
{
    public GameObject newGameDialog;

    public void OnContinueClicked()
    {
        string path = Application.persistentDataPath + "/save.txt";
        if (File.Exists(path))
        {
	        SceneManager.LoadScene("L1");
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
	        SceneManager.LoadScene("L1");
        }
    }
}
