using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using MushroomBox.Debug;

public class TitleScreen : MonoBehaviour
{
    public GameObject newGameDialog;
    public Slider slider;
    public AsyncOperation scene;
    public bool isLoaded = false;
    public Animator fadeAnim;

    // Start is called before the first frame update
    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        string path = Application.persistentDataPath + "/save.txt";
        if (!File.Exists(path))
        {
	        buttons[0].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
	        buttons[1].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        }
        else
        {
            buttons[0].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
            buttons[1].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
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
            StartCoroutine("LoadL1Co", 3);
        }
    }

    public void LoadL1()
    {
        if (isLoaded) {
            scene.allowSceneActivation = true;
        } else {
            this.D("C");
        }
    }

    IEnumerator LoadL1Co(int delay = 0)
    {
        yield return new WaitForSeconds(delay);

        scene = SceneManager.LoadSceneAsync("L1");
//        scene.allowSceneActivation = false;

        while (!scene.isDone)
        {

            float progress = Mathf.Clamp01(scene.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }


        this.D("A");
        fadeAnim.SetTrigger("FadeOut");
        this.D("B");
        yield return new WaitForSeconds(1);
        scene.allowSceneActivation = true;
    }
}
