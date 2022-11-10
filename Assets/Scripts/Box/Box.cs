using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MushroomBox.Debug;

public class Box : MonoBehaviour
{
    public int state;
    public SpriteRenderer boxSprite;
    public Sprite[] boxSprites;
    public SpriteRenderer bubble;
    public Sprite[] bubbleSprites;

    public int progress;
    public bool isTarget;

    private void Awake()
    {
        state = BoxState.Empty;
        boxSprite = GetComponent<SpriteRenderer>();
        SetSprite(BoxSprite.Empty);
        SetBubbleSprite(BoxBubbleSprite.None);
        progress = 0;
        isTarget = false;
    }

    public void SetSprite(int s)
	{
		if (s >= boxSprites.Length)
			return;
			
        boxSprite.sprite = boxSprites[s];
    }

    public void SetBubbleSprite(int s)
    {
        bubble.sprite = bubbleSprites[s];
    }

    public void UpdateSprite()
    {
        if (state == BoxState.Empty)
        {
            SetBubbleSprite(BoxBubbleSprite.None);
            return;
        }
        else if (state == BoxState.Growing)
        {
            SetBubbleSprite(BoxBubbleSprite.None);
        }
        else if (state == BoxState.Done)
        {
            SetBubbleSprite(BoxBubbleSprite.Mushroom1);
            SetSprite(BoxSprite.Done);
            return;
        }

        SetSprite(progress);

        StartCoroutine("AnimateGrowth");
    }

    public void SetState(int s, bool update = false)
    {
        state = s;
        SetProgress(progress);
        if (update)
            UpdateSprite();
    }

    public void StartGrowingProgress()
    {
        SetProgress(0);
        SetState(BoxState.Growing);
        SetProgress(1);
        SetSprite(BoxSprite.Growing1);

        StartCoroutine("AnimateGrowth");
    }

    public void SetProgress(int p)
    {
        progress = p;
        SaveSystem.SaveGame();
    }

    IEnumerator AnimateGrowth()
    {
        yield return new WaitForSeconds(5f);

        if (state == BoxState.Empty)
        {
            yield break;
        }

        if (progress < 8)
        {
            SetSprite(progress);
        } else
        {
            SetState(BoxState.Done);
            SetBubbleSprite(BoxBubbleSprite.Mushroom1);
            yield break;
        }

        SetProgress(progress + 1);

        if (progress <= 8)
        {
            StartCoroutine("AnimateGrowth");
        }
    }

    public void OnMouseUp()
    {
        if (Game.player.GetDirection() == Direction.Right)
        {
            if (Vector2.Distance(Game.player.transform.position, Game.player.startingPos) < 5)
                return;
        }

        if (bubble.sprite == bubbleSprites[0])
            return;

        if (state == BoxState.Done)
        {
	        Game.player.boxQueue.Enqueue(this);
            this.D("Enqueue: " + gameObject.name, gameObject);
            this.D("isMoving: " + Game.player.isMoving + ", walkingBoxes: " + Game.player.walkingBoxes, gameObject);
	        SetBubbleSprite(BoxBubbleSprite.None);
            if (!Game.player.isMoving && !Game.player.walkingBoxes)
                Game.player.WalkToBoxes();
        }


    }
}
