using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDialogHandler : MonoBehaviour
{
    public TextMeshProUGUI[] costTexts;

    public void Start()
    {
        StartCoroutine("StartDelayed", 2);
    }

    IEnumerator StartDelayed(int s)
    {
        yield return new WaitForSeconds(s);

        foreach (TextMeshProUGUI costText in costTexts)
        {
            if (Game.coinCount >= int.Parse(costText.text))
            {
                costText.color = new Color(0f, 1f, 0f, 1f);
            }
            else
            {
                costText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        StartCoroutine("StartDelayed", 2);
    }

    public void Update()
    {

    }

}
