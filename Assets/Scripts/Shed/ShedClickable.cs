using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MushroomBox.Debug;

public class ShedClickable : MonoBehaviour
{
    public int id = 0;
    public SpriteRenderer bubble;

    void Start()
    {

    }

    public void OnMouseUp()
    {
        this.D("shed clickable click: " + id);

	    if (id <= 16) {
	        if (Game.coinCount >= 30)
	        {
	            if (bubble.gameObject.activeSelf == false)
	            {
	                bubble.gameObject.SetActive(true);
	                StartCoroutine("DelayCloseBubble");
	                return;
	            }
	            else 
	            {
		            this.D("shed item: " + id + " paid");
	                Game.counter.coinChange(-30);
	                Game.goalDisplays[0].goal.Value = Game.goalDisplays[0].goal.Value + 1;
	                Game.goalDisplays[0].RefreshData();
	                Game.shedItems[id] = false;
		            if (Game.goalDisplays[0].goal.Value == 19) 
	                {
	                    // todo: show end of game dialog
	                    GameObject.Find("EndDialog").GetComponent<Animator>().SetBool("Idle", false);
	                    GameObject.Find("EndDialog").GetComponent<Animator>().SetBool("Extend", true);
	                }
	                SaveSystem.SaveGame();
	                gameObject.SetActive(false);
	                bubble.gameObject.SetActive(false);
	            }
	        }
	    }
	    else
	    {
		    if (Game.coinCount >= 100)
		    {
			    if (bubble.gameObject.activeSelf == false)
			    {
				    bubble.gameObject.SetActive(true);
				    StartCoroutine("DelayCloseBubble");
				    return;
			    }
			    else
			    {
				    this.D("shed item: " + id + " paid");
				    Game.counter.coinChange(-100);
				    Game.goalDisplays[0].goal.Value = Game.goalDisplays[0].goal.Value + 1;
				    Game.goalDisplays[0].RefreshData();
				    Game.shedItems[id] = false;
				    if (Game.goalDisplays[0].goal.Value == 19) 
				    {
					    // todo: show end of game dialog
					    GameObject.Find("EndDialog").GetComponent<Animator>().SetBool("Idle", false);
					    GameObject.Find("EndDialog").GetComponent<Animator>().SetBool("Extend", true);
				    }
				    SaveSystem.SaveGame();
				    gameObject.SetActive(false);
				    bubble.gameObject.SetActive(false);
			    }
		    }
	    }
    }

    public IEnumerator DelayCloseBubble()
    {
        yield return new WaitForSeconds(2);
        if (bubble.gameObject.activeSelf == true)
        {
            bubble.gameObject.SetActive(false);
        }
    }
}
