using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "New Goal", menuName = "Goal")]
public class Goal : ScriptableObject
{
    public Sprite Sprite;
    public string Text;
    public int Minimum;
    public int Maximum;
    public int Value;
    public bool Complete;
}
