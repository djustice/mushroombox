using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Vector3 destJarPos;
    Vector3 destBoxPos;

	public bool collectNextJars;
	public bool walkingToStartPos = false;
    public bool customerWaiting = false;

    public Queue<Jar> jarQueue;
    public Queue<Box> boxQueue;

    public Box nextBox;

    public Vector3 destination;

    void Start()
    {
        startingPos = new Vector3(transform.position.x, transform.position.y, 0);

        jarQueue = new Queue<Jar>();
        boxQueue = new Queue<Box>();

        isMoving = false;

        SetIdle(true);

        speed = 350f;
    }

    public void MoveCakes()
    {
        StartCoroutine("WalkToJars");
    }

    IEnumerator WalkToJars()
    {
        if (!isMoving)
        {
            if (jarQueue.Count > 0)
            {
                nextBox = Game.getNextAvailBox();
                if (nextBox != null)
                {
                    isMoving = true;
                    yield return StartCoroutine("WalkJarToBox", jarQueue.Dequeue());
                }
            }

            if (jarQueue.Count > 0)
            {
                MoveCakes();
            }
            else
            {
                if (boxQueue.Count > 0)
                {
                    WalkToBoxes();
                }
            }
        }
    }

    public void WalkToBoxes()
    {
        if (!isMoving)
        {
            StartCoroutine("HarvestBox", boxQueue.Dequeue());
        }
    }

    IEnumerator WalkJarToBox(Jar jar)
    {
        Vector3 jarPos = new Vector2(jar.transform.position.x + 100, jar.transform.position.y + 16);

        Box nextBox = Game.getNextAvailBox();
        if (nextBox != null)
        {
            nextBox.isTarget = true;
            Vector3 destBoxPos = new Vector3(nextBox.transform.position.x, nextBox.transform.position.y - 60);

            yield return StartCoroutine("IMoveTo", jarPos);
            yield return StartCoroutine("MoveLeftTo", jarPos);
            yield return new WaitForSeconds(1f);
            jar.ResetJar();
	        yield return StartCoroutine("IMoveTo", destBoxPos);
            
            if (Vector2.Distance(transform.position, destBoxPos) > 50)
            {
                yield return StartCoroutine("MoveUpTo", destBoxPos);
            }
            SetDirection(Direction.Up);
            yield return new WaitForSeconds(1f);
            nextBox.StartGrowingProgress();
            nextBox.isTarget = false;

            if (jarQueue.Count == 0)
            {
                if (boxQueue.Count == 0)
                {
                    Game.player.SetDirection(Direction.Down);
                    Game.player.SetIdle(true);

                    isMoving = false;
                    if (customerWaiting == true)
                    {
                        WalkToDesk();
                    }
                }
                else
                {
                    yield return StartCoroutine("HarvestBox", boxQueue.Dequeue());
                }
            }

            isMoving = false;
        }
    }

    IEnumerator HarvestBox(Box box)
    {
        if (box.state != BoxState.Done)
            yield break;

        if (isMoving)
            yield break;

        isMoving = true;
        
        Vector3 destBoxPos = new Vector3(box.transform.position.x, box.transform.position.y - 60);
        yield return StartCoroutine("IMoveTo", destBoxPos);
        yield return StartCoroutine("MoveUpTo", destBoxPos);

        yield return new WaitForSeconds(1f);

        box.GetComponent<Animator>().SetTrigger("AddMushroom");
        box.boxSprite.sprite = box.boxSprites[0];
        box.bubble.sprite = box.bubbleSprites[0];
        box.SetState(BoxState.Empty);
        box.SetProgress(0);
        box.isTarget = false;

	    Game.counter.mushroomChange(1);

	    int sporeReward = Random.Range(1, Game.sporeRewardFactor + 1);
	    if (sporeReward == 1) 
	    {
	    	Game.counter.sporeChange(1);
	    }
	    
        if (boxQueue.Count == 0)
        {
            Debug.Log("Player : Harvest Box : boxQueue.Count : " + boxQueue.Count);
            if (jarQueue.Count > 0)
            {
                Debug.Log("Player : Harvest Box : jarQueue.Count : " + jarQueue.Count);
                yield return StartCoroutine("WalkJarToBox", jarQueue.Dequeue());
            }
            Debug.Log("Player : Harvest Box : Done");
            Game.player.SetDirection(Direction.Down);
            Game.player.SetIdle(true);

            isMoving = false;
            if (customerWaiting == true)
            {
                WalkToDesk();
            }
        } else
        {
            isMoving = false;
            yield return StartCoroutine("HarvestBox", boxQueue.Dequeue());
        }

        isMoving = false;
    }

    public void WalkToDesk()
    {
        if (!isMoving) 
        {
            StartCoroutine("IWalkToDesk");
        }
    }

    IEnumerator IWalkToDesk()
    {
        isMoving = true;

        yield return StartCoroutine("IMoveTo", startingPos);
        yield return StartCoroutine("MoveDownTo", startingPos);

        isMoving = false;
    }

    public void Walk(int direction, Vector3 dest)
    {
        destination = dest;
        StartCoroutine("IWalk", direction);
    }

    IEnumerator IWalk(int direction)
    {
        if (isMoving)
            yield break;

        isMoving = true;

        if (direction == 0)
        {
            yield return StartCoroutine("MoveDownTo", destination);
        }
        else if (direction == 1)
        {
            yield return StartCoroutine("MoveLeftTo", destination);
        }
        else if (direction == 2)
        {
            yield return StartCoroutine("MoveRightTo", destination);
        }
        else if (direction == 3)
        {
            yield return StartCoroutine("MoveUpTo", destination);
        }

        isMoving = false;
        SetDirection(Direction.Down);
    }
}
