using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using MushroomBox.Debug;

public class TitleScreenButtonHandler : MonoBehaviour
{
    public GameObject newGameDialog;
    public Slider slider;
    public bool isLoaded = false;
    public AsyncOperation scene;
    public Animator fadeAnim;

    public void OnContinueClicked()
    {
        string path = Application.persistentDataPath + "/save.txt";
        if (File.Exists(path))
        {
            slider.gameObject.SetActive(true);
            StartCoroutine("LoadL1Co");
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
            slider.gameObject.SetActive(true);
            StartCoroutine("LoadL1Co");
        }
    }

    public void LoadL1() {
        if (isLoaded)
        {
            scene.allowSceneActivation = true;
        }
        else
        {
            this.D("C");
        }
    }

    IEnumerator LoadL1Co()
    {
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
        isLoaded = true;
        yield return new WaitForSeconds(1);
        LoadL1();
    }
}
