using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int coinCount;
    public int mushroomCount;
    public int sporeCount;
    public int jarCount;
    public int[] jarProgresses;
    public int[] jarStates;
    public int boxCount;
    public int[] boxProgresses;
    public int[] boxStates;

    public GameData()
    {
        coinCount = Game.coinCount;
        mushroomCount = Game.mushroomCount;
        sporeCount = Game.sporeCount;
        jarCount = Game.jars.Count;
        jarProgresses = new int[jarCount];
        int jpi = 0;
        while (jpi < jarCount)
        {
            jarProgresses[jpi] = Game.jars[jpi].progress;
            jpi++;
        }
        jarStates = new int[jarCount];
        int jsi = 0;
        while (jsi < jarCount)
        {
            jarStates[jsi] = (int)Game.jars[jsi].state;
            jsi++;
        }
        boxCount = Game.boxes.Count;
        boxProgresses = new int[boxCount];
        int bpi = 0;
        while (bpi < boxCount)
        {
            boxProgresses[bpi] = Game.boxes[bpi].progress;
            bpi++;
        }
        boxStates = new int[boxCount];
        int bsi = 0;
        while (bsi < boxCount)
        {
            boxStates[bsi] = (int)Game.boxes[bsi].state;
            bsi++;
        }
    }
}
