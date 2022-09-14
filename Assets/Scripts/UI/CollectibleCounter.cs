using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public Animator coinAnim;
    public TextMeshProUGUI mushroomText;
    public Animator mushroomAnim;
    public TextMeshProUGUI sporeText;
    public Animator sporeAnim;

    public void Awake()
    {
        coinText = GameObject.Find("CoinCountText").GetComponent<TextMeshProUGUI>();
        mushroomText = GameObject.Find("MushroomCountText").GetComponent<TextMeshProUGUI>();
        sporeText = GameObject.Find("SporeCountText").GetComponent<TextMeshProUGUI>();
    }

    public void coinChange(int coins, bool set = false)
    {
        if (set == true)
        {
            Game.coinCount = coins;
        }
        else
        {
            Game.coinCount = Game.coinCount + coins;
        }

        coinAnim.SetTrigger("CoinChange");
        coinText.text = Game.coinCount.ToString();
        SaveSystem.SaveGame();
    }

    public void mushroomChange(int mushrooms, bool set = false)
    {
        if (set == true)
        {
            Game.mushroomCount = mushrooms;
        }
        else
        {
            Game.mushroomCount = Game.mushroomCount + mushrooms;
        }

        mushroomAnim.SetTrigger("MushroomChange");
        mushroomText.text = Game.mushroomCount.ToString();
        SaveSystem.SaveGame();
    }

    public void sporeChange(int spores, bool set = false)
    {
        if (set == true)
        {
            Game.sporeCount = spores;
        }
        else
        {
            Game.sporeCount= Game.sporeCount + spores;
        }

        sporeAnim.SetTrigger("SporeChange");
        sporeText.text = Game.sporeCount.ToString();
        SaveSystem.SaveGame();
    }
}
