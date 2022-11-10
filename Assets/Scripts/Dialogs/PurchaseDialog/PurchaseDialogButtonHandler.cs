using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MushroomBox.Debug;

public class PurchaseDialogButtonHandler : MonoBehaviour
{
    public GameObject purchaseDialog;
    public ShopItem[] items;
    public Image[] images;
    public TextMeshProUGUI[] costTexts;
    public TextMeshProUGUI item;

    public void Start()
    {
        item = purchaseDialog.GetComponentInChildren<TextMeshProUGUI>();
        //StartCoroutine("StartDelayed", 2);
    }

    IEnumerator StartDelayed(int s)
    {
        yield return new WaitForSeconds(s);

        foreach (TextMeshProUGUI costText in costTexts)
        {
            if (Game.coinCount >= int.Parse(costText.text))
            {
                costText.color = new Color(0f, 0.5f, 0f, 1f);
            }
            else
            {
                costText.color = new Color(0.6f, 0f, 0f, 1f);
            }
        }

        StartCoroutine("StartDelayed", 2);
    }

    public void OnCloseButtonClicked()
	{
		this.D("PurchaseDialog : OnCloseClicked()");
		purchaseDialog.GetComponent<Animator>().SetInteger("state", 2);
	}
    
	public void HideImages() 
	{
		item.text = "";
		foreach(Image i in images)
		{
			i.gameObject.SetActive(false);
		}
	}

    public void OnPurchaseButtonClicked()
	{
		//this.D("PurchaseDialog : OnPurchaseClicked()");
        //foreach (Image i in images)
        //{
        //    if(i.gameObject.activeSelf == true)
        //    {
        //        if (item.text.StartsWith("Spore"))
        //        {
        //            int cost = int.Parse(items[0].costText.text);
        //            int quant = int.Parse(items[0].quantityText.text.Split('x')[0]);

        //            if (Game.coinCount >= cost)
        //            {
        //                Game.counter.coinChange(-cost);
        //                Game.counter.sporeChange(quant);
        //                OnCloseButtonClicked();
        //            }
        //        }
        //        else if (item.text == "Box")
        //        {
        //            if (Game.coinCount >= 300)
        //            {
        //                Game.counter.coinChange(-300);

        //                Vector3 pos = Game.boxPlaceholder.transform.position;
        //                Game.boxPlaceholder.transform.position = new Vector3(pos.x + 141, pos.y);
        //                Box newBox = Instantiate<Box>(FindObjectOfType<Box>(), Game.boxes[0].transform.parent, false);
        //                newBox.transform.position = pos;
        //                newBox.SetProgress(0);
        //                newBox.SetState(BoxState.Empty);
        //                newBox.SetSprite(BoxSprite.Empty);
        //                newBox.SetBubbleSprite(BoxBubbleSprite.None);
        //                Game.boxes.Add(newBox);

        //                if (Game.boxes.Count == 4)
        //                    Game.boxPlaceholder.gameObject.SetActive(false);

        //                SaveSystem.SaveGame();
        //                OnCloseButtonClicked();
        //            }
        //        }
        //        else if (item.text == "Jar")
        //        {
        //            if (Game.coinCount >= 100)
        //            {
        //                Game.counter.coinChange(-100);

        //                Vector3 pos = Game.jarPlaceholder.transform.position;
        //                Game.jarPlaceholder.transform.position = new Vector3(pos.x, pos.y - 150);
        //                Jar newJar = Instantiate<Jar>(FindObjectOfType<Jar>(), Game.jars[0].transform.parent, false);
        //                newJar.transform.position = pos;
        //                newJar.SetProgress(0);
        //                newJar.SetState(JarState.Empty);
        //                newJar.SetSprite(JarSprite.Empty);
        //                newJar.SetBubbleSprite(JarBubbleSprite.Substrate);
        //                newJar.ShakeAnimation();
        //                Game.jars.Add(newJar);
                        
        //                if (Game.jars.Count == 3)
        //                    Game.jarPlaceholder.gameObject.SetActive(false);

        //                SaveSystem.SaveGame();
        //                OnCloseButtonClicked();
        //            }
        //        }
        //    }
        //}
    }
    
	public void UpdateCostColor()
	{
		this.D("update.dlg.txt.color");
		foreach (TextMeshProUGUI costText in costTexts)
		{
			this.D("update.dlg.txt.color.2");
			if (Game.coinCount >= int.Parse(costText.text))
			{
				costText.color = new Color(0f, 1f, 0f, 1f);
			}
			else
			{
				costText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}
	}
}
