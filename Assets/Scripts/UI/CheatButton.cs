using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatButton : MonoBehaviour
{
	public void OnMouseUp()
	{
		Game.counter.mushroomChange(4);
	}
}
