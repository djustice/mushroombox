using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using MushroomBox.Debug;

public class Jar : MonoBehaviour
{
    public int state;

    public int progress;
    public SpriteRenderer sprite;
    public Sprite[] progressSprites;
    public SpriteRenderer bubble;
    public Sprite[] bubbleSprites;

    public SpriteRenderer sporeAnim;

	public Box nextBox;
    
	public int tapCount;
	public Transform floatingText;

    private void Awake()
	{
		// this.D("Jar /" + this.name + "/ : Awake");
		
        state = JarState.Empty;
        bubble.sprite = bubbleSprites[0];
		sprite = GetComponent<SpriteRenderer>();
	}

    public void SetProgress(int p)
	{
		// this.D("Jar /" + this.name + "/ : SetProgress : " + p);
		
        progress = p;
        SaveSystem.SaveGame();
    }

    public void SetState(int s, bool update = false)
	{
		// this.D("Jar /" + this.name + "/ : SetState : " + s + " : update : " + update);
        state = s;
        SetProgress(progress);
        if (update)
            UpdateSprite();
    }

    public void SetBubbleSprite(int s)
	{
		// this.D("Jar /" + this.name + "/ : SetBubbleSprite : " + s);
		
        bubble.sprite = bubbleSprites[s];
    }

    public void SetSprite(int s)
	{
		// this.D("Jar /" + this.name + "/ : SetSprite : " + s);
		
        sprite.sprite = progressSprites[s];
    }
    
    IEnumerator StartProgress()
	{
		// this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime);
		
		int intervalChange = 0;
		if (tapCount > 5) 
		{
			// this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : tapCount : >5");
			intervalChange = tapCount / 5;

			if (intervalChange > 4)
			{
				// this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : intervalChange : " + intervalChange);
				intervalChange = 4;
			}
		}
		
		yield return new WaitForSeconds(5f - intervalChange);
		// this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : waited seconds : " + (5 - intervalChange));
		
        if (progress > JarSprite.Substrate && progress < JarSprite.Growing5)
        {
	        // this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : progress : > Substrate && < Growing5");
	        
            SetBubbleSprite(JarBubbleSprite.None);
            StartCoroutine("StartProgress");
        }
        else if (progress == JarSprite.Growing5)
        {
	        // this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : progress : > Growing5");
	        
            GetComponent<Animator>().SetTrigger("Bounce");
            SetBubbleSprite(JarBubbleSprite.Water);
        }
        else if (progress == JarSprite.Done)
        {
	        // this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : progress : Done");
	        
            GetComponent<Animator>().SetBool("Idle", false);
            GetComponent<Animator>().SetTrigger("Done");
            SetBubbleSprite(JarBubbleSprite.Check);
        }

        SetProgress(progress + 1);

        if (progress < progressSprites.Length)
        {
	        // this.D("Jar /" + this.name + "/ : StartProgress : " + Time.fixedTime + " : progress : < progressSprites.Length");
	        
            SetSprite(progress);
        }
    }

    public void SporeAnimation(bool active)
	{
		// this.D("Jar /" + this.name + "/ : SporeAnimation : active : " + active);
        if (!active)
        {
            sporeAnim.GetComponent<Animator>().SetInteger("state", 0);
        } 
        else
        {
            sporeAnim.GetComponent<Animator>().SetInteger("state", 1);
        }
    }

    public void ShakeAnimation()
	{
		// this.D("Jar /" + this.name + "/ : ShakeAnimation");
		
        GetComponent<Animator>().SetTrigger("Shake");
    }

    public void BounceAnimation()
	{
		// this.D("Jar /" + this.name + "/ : BounceAnimation");
		
        GetComponent<Animator>().SetTrigger("Bounce");
    }
    
	public void TapAnimation()
	{
		// this.D("Jar /" + this.name + "/ : TapAnimation");
		
		GetComponent<Animator>().SetTrigger("Tap");
	}

    public void DoneAnimation(bool active)
	{
		// this.D("Jar /" + this.name + "/ : DoneAnimation : active : " + active);
		
        if (active)
        {
            GetComponent<Animator>().SetBool("Idle", false);
            GetComponent<Animator>().SetTrigger("Done");
        }
        else
        {
            GetComponent<Animator>().SetBool("Idle", true);
        }
    }

    public void OnMouseUp()
	{
        if (Game.player.walkingBoxes == true)
            return;

		// this.D("Jar /" + this.name + "/ : OnMouseUp :");
        if (Game.player.GetDirection() == Direction.Right)
        {
	        // this.D("Jar /" + this.name + "/ : OnMouseUp : player facing right : true");
	        if (Vector2.Distance(Game.player.transform.position, Game.player.startingPos) < 5) 
	        {
		        // this.D("Jar /" + this.name + "/ : OnMouseUp : player at startingPos(+-5) : true");
	        	return;
	        }
	        // this.D("Jar /" + this.name + "/ : OnMouseUp : player at startingPos(+-5) : false");
        }
        
        if (Game.player.jarQueue.Contains(this))
        {
            return;
        }

        if (sprite.sprite == progressSprites[JarSprite.Done] && bubble.sprite == bubbleSprites[JarBubbleSprite.None])
        {
            return;
        }


        if (Game.player.walkingToStartPos == true)
		{
			// this.D("Jar /" + this.name + "/ : OnMouseUp : player walking to start pos : true");
			return;
		}
        
		// this.D("Jar /" + this.name + "/ : OnMouseUp : player facing right : false");

        if (state == JarState.Empty)
        {
	        // this.D("Jar /" + this.name + "/ : OnMouseUp : jarState : empty");

	        SetProgress(1);
            SetState(JarState.Fill);
            SetSprite(JarSprite.Substrate);
            SetBubbleSprite(JarBubbleSprite.Spore);
            SporeAnimation(false);
            return;
        }
        
		// this.D("Jar /" + this.name + "/ : OnMouseUp : jarState : !empty");

        if (state == JarState.Fill)
        {
	        // this.D("Jar /" + this.name + "/ : OnMouseUp : jarState : fill");
            if (Game.sporeCount >= 1)
            {
	            // this.D("Jar /" + this.name + "/ : OnMouseUp : sporeCount : >= 1");
                SetProgress(2);
                SetState(JarState.Growing);
                SetBubbleSprite(JarBubbleSprite.None);
                SporeAnimation(true);
                StartCoroutine("StartProgress");
                Game.counter.sporeChange(-1);
            }
        }

        if (state == JarState.Growing)
        {
	        // this.D("Jar /" + this.name + "/ : OnMouseUp : jarState : growing");
	        
            if (bubble.sprite == bubbleSprites[3])
            {
	            // this.D("Jar /" + this.name + "/ : OnMouseUp : bubbleSprite : 3");
                DoneAnimation(true);
                SetBubbleSprite(JarBubbleSprite.Check);
                SetState(JarState.Done);
                return;
            }

	        // this.D("Jar /" + this.name + "/ : OnMouseUp : bubbleSprite : !3");

	        // Transform newFloatingText = Instantiate(floatingText);
	        // newFloatingText.gameObject.SetActive(true);
	        // newFloatingText.SetParent(this.transform);
	        // newFloatingText.GetComponent<Animator>().SetTrigger("Popup");

	        // tapCount++;

	        // if (tapCount >= 19 && tapCount <= 20)
	        // {
		    //     // this.D("Jar /" + this.name + "/ : OnMouseUp : tapCount : " + tapCount);
		    //     Game.counter.coinChange(-1);
	        // }

	        TapAnimation();
        }

        if (state == JarState.Done)
        {
	        // this.D("Jar /" + this.name + "/ : OnMouseUp : jarState : done");
            foreach (Box box in Game.boxes)
            {
                if (box.state == BoxState.Empty && box.isTarget == false)
                {
                    nextBox = box;
                    break;
                }
                else
                {
                    nextBox = null;
                }
            }

	        if (nextBox == null) 
	        {
		        // this.D("Jar /" + this.name + "/ : OnMouseUp : no empty box avail");
                Game.player.NotifyNoBoxAvailable();
		        return;
	        }

	        // this.D("Jar /" + this.name + "/ : OnMouseUp : found empty box : " + nextBox.name);

            nextBox.isTarget = true;
            SetProgress(0);
            SetState(JarState.Empty);
            DoneAnimation(false);
            SetBubbleSprite(JarBubbleSprite.None);

            Game.player.jarQueue.Enqueue(this);
            if (!Game.player.isMoving && !Game.player.customerWalking && !Game.player.walkingJars)
    	        Game.player.MoveCakes();
            
	        tapCount = 0;
        }
    }

    public void UpdateSprite()
    {
        if (state == JarState.Empty)
        {
            SetBubbleSprite(JarBubbleSprite.Substrate);
            BounceAnimation();
            return;
        } else if (state == JarState.Fill)
        {
            SetBubbleSprite(JarBubbleSprite.Spore);
            BounceAnimation();
        } else if (state == JarState.Growing)
        {
            if (progress < JarSprite.Growing5)
            {
                SetBubbleSprite(JarBubbleSprite.None);
            }
            else if (progress == JarSprite.Growing5)
            {
                SetBubbleSprite(JarBubbleSprite.Water);
                BounceAnimation();
            }
            else if (progress == JarSprite.Done)
            {
                SetBubbleSprite(JarBubbleSprite.Water);
                SetSprite(JarSprite.Done);
                BounceAnimation();
            }
        } else if (state == JarState.Done)
        {
            SetBubbleSprite(JarBubbleSprite.Check);
            SetSprite(JarSprite.Done);
            DoneAnimation(true);
        }

        if (progress < JarSprite.Done)
        {
            SetSprite(progress);
            StartCoroutine("StartProgress");
        }
    }

    public void ResetJar()
    {
        SetBubbleSprite(JarBubbleSprite.Substrate);
        SetProgress(0);
        SetSprite(JarSprite.Empty);
        DoneAnimation(false);
    }
}
