using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDisplayEffect : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
	public SpriteRenderer elipses;
    public string[] texts;
    public bool isIdle = false;
    private TextDialog textDialog;
    public int currentPage = 0;
    private string pageText;
    private string currentText;

    public void Start()
    {
        textDialog = FindObjectOfType<TextDialog>();
    }

    public void PlayPage()
    {
        if (currentPage == texts.Length)
        {
            textDialog.isActive = false;
            textDialog.CloseDialog();
            return;
        }

        textMesh.text = "";
        StartCoroutine("ShowText");
    }

    IEnumerator ShowText()
    {
        isIdle = false;
        elipses.GetComponent<Animator>().SetBool("Show", false);

        pageText = texts[currentPage];

        if (pageText == "[Event]")
        {
            textDialog.eventFlag = true;
            currentPage++;
            yield break;
        }

        if (textDialog.eventFlag == true)
        {
            yield return StartCoroutine("WaitOnEventFlag");
        }

        for (int i = 0; i <= pageText.Length; i++)
        {
            currentText = pageText.Substring(0, i);
            textMesh.text = currentText;
            if (currentText.EndsWith('.') || currentText.EndsWith('!') || currentText.EndsWith('?'))
            {
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(0.03f);
        }

        if (currentPage < texts.Length)
        {
            currentPage++;
            if (currentPage == texts.Length)
            {
                isIdle = true;
                elipses.GetComponent<Animator>().SetBool("Show", true);
                yield break;
            }

            pageText = texts[currentPage];
        }

        isIdle = true;
        elipses.GetComponent<Animator>().SetBool("Show", true);
    }

    IEnumerator WaitOnEventFlag()
    {
        yield return new WaitForSeconds(0.4f);

        if (textDialog.eventFlag == false)
        {
            PlayPage();
            yield break;
        }

        StartCoroutine("WaitOnEventFlag");
    }
}
