using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MushroomBox.Debug;

public class Customer : Character
{
    public GameObject[] path;

    Vector3[] points;

    void Start()
    {
        startingPos = transform.position;
        isMoving = false;
        points = new Vector3[path.Length];
        int i = 0;
        foreach (GameObject g in path)
        {
            points[i] = g.transform.position;
            i++;
        }
        speed = 350f;
    }
    
	public void StartCheckForMushrooms() 
	{
		StartCoroutine("CheckForMushrooms");
	}

    IEnumerator CheckForMushrooms()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        if (Game.mushroomCount > 0)
        {
            if (Vector3.Distance(transform.position, startingPos) < 5 || Vector3.Distance(transform.position, points[4]) < 5)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    StartCoroutine("WalkPath");
                }
            } else
            {
                this.D(this + " pos > 5, t.pos: " + transform.position + ", p[4]: " + points[4]);
            }
        }
    }

    IEnumerator WalkPath()
	{
        Game.player.customerWalking = true;
        this.D(this.name + " walk");
        Game.walkableArea.gameObject.SetActive(false);
		transform.position = points[0];
        Game.player.customerWaiting = true;
        if (!Game.player.walkingJars && !Game.player.walkingBoxes)
            Game.player.WalkToDesk();
        yield return StartCoroutine("MoveLeftTo", points[1]);
        yield return StartCoroutine("MoveUpTo", points[2]);
        SetDirection(Direction.Left);
        yield return StartCoroutine("WaitOnPlayer");
        bubble.sprite = bubbleSprites[1];
        Game.player.SetDirection(Direction.Right);
        yield return StartCoroutine("HideInquiryBubble");
        yield return StartCoroutine("ShowCoinBubble");
        yield return StartCoroutine("ProcessSale");
        yield return StartCoroutine("HideCheckBubble");
        Game.player.customerWaiting = false;
        Game.player.customerWalking = false;
        Game.walkableArea.gameObject.SetActive(true);
        if (Game.player.jarQueue.Count > 0 && !Game.player.walkingBoxes)
        {
            Game.player.MoveCakes();
        }
		if (Game.player.boxQueue.Count > 0)
		{
			Game.player.WalkToBoxes();
		}
        yield return StartCoroutine("MoveDownTo", points[3]);
        yield return StartCoroutine("MoveLeftTo", points[4]);
        // transform.position = points[0];
        isMoving = false;
        this.D(this.name + " walk done");
    }

    IEnumerator WaitOnPlayer()
    {
        while (Vector2.Distance(Game.player.transform.position, Game.player.startingPos) > 5)
        {
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
 
    IEnumerator HideInquiryBubble()
    {
        yield return new WaitForSeconds(1f);

        if (bubble.sprite == bubbleSprites[1])
        {
            bubble.sprite = bubbleSprites[0];
        }
    }

    IEnumerator ShowCoinBubble()
    {
        yield return new WaitForSeconds(0.05f);

        Game.player.bubble.sprite = Game.player.bubbleSprites[1];
    }

    IEnumerator ProcessSale()
    {
        yield return new WaitForSeconds(1f);

        if (Game.player.bubble.sprite == Game.player.bubbleSprites[1])
        {
            Game.player.bubble.sprite = Game.player.bubbleSprites[0];
            bubble.sprite = bubbleSprites[2];
            Game.counter.mushroomChange(-1);
            Game.coinCountTotal = Game.coinCountTotal + 10;
            Game.counter.coinChange(10);
        }
    }

    IEnumerator HideCheckBubble()
    {
        yield return new WaitForSeconds(1f);

        SetIdle(false);
        SetDirection(Direction.Down);

        Game.player.SetDirection(Direction.Down);

        bubble.sprite = bubbleSprites[0];
    }

}
