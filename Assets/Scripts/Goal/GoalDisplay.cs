using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MushroomBox.Debug;

public class GoalDisplay : MonoBehaviour
{
    public Goal goal;

    void Start()
    {
        Image i = GetComponentsInChildren<Image>()[1];
        i.sprite = goal.Sprite;

        TextMeshProUGUI text = GetComponentsInChildren<TextMeshProUGUI>()[0];
        text.text = goal.Text;

        TextMeshProUGUI completedText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        completedText.text = goal.Value + " / " + goal.Maximum;
    }

    public void RefreshData()
    {
        this.D("goal refresh " + goal.Value);
        Image i = GetComponentsInChildren<Image>()[1];
        i.sprite = goal.Sprite;

        TextMeshProUGUI text = GetComponentsInChildren<TextMeshProUGUI>()[0];
        text.text = goal.Text;

        TextMeshProUGUI completedText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        completedText.text = goal.Value + " / " + goal.Maximum;
        this.D("goal refresh " + completedText.text);
    }
}
