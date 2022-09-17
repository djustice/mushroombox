using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject newGameDialog;

    // Start is called before the first frame update
    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        string path = Application.persistentDataPath + "/save.txt";
        if (!File.Exists(path))
        {
	        buttons[0].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
	        buttons[1].GetComponent<Image>().color = new Color(0.5f, 0f, 0f, 0.6f);
        }
        else
        {
            buttons[0].GetComponent<Image>().color = new Color(0.5f, 0f, 0f, 0.6f);
            buttons[1].GetComponent<Image>().color = new Color(0.5f, 0f, 0f, 0.6f);
        }
    }

    public void OnCancelMouseUp()
    {
        newGameDialog.SetActive(false);
    }

    public void OnOkMouseUp()
    {
        string path = Application.persistentDataPath + "/save.txt";
        if (File.Exists(path))
        {
            File.Delete(path);
	        SceneManager.LoadScene("L1");
        }
    }
}
