using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MushroomBox.Debug;

public class Player : Character
{
    Vector3 destJarPos;
    Vector3 destBoxPos;

	public bool walkingJars = false;
    public bool walkingBoxes = false;
	public bool walkingToStartPos = false;
    public bool customerWaiting = false;
    public bool customerWalking = false;

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
        this.D("MoveCakes() called: " + Time.fixedTime);
        this.D("MoveCakes() called: cusWalk: " + customerWalking + ", cusWait: " + customerWaiting);
        if (!customerWalking && !customerWaiting && !walkingBoxes)
        {
            StartCoroutine("WalkToJars");
        } 
        else
        {
            WalkToDesk();
        }
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
                    this.D("walkingJars true");
                    walkingJars = true;
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
                walkingJars = false;
                this.D("walkingJars false");

                if (boxQueue.Count > 0)
                {
                    WalkToBoxes();
                }
                else
                {
                    this.D("empty jarQ && empty boxQ", gameObject);

                    walkingBoxes = false;

                    if (customerWaiting == true)
                    {
                        WalkToDesk();
                    }
                }
            }
        }
    }

    public void WalkToBoxes()
    {
        if (!isMoving && !customerWalking)
        {
            walkingBoxes = true;
            this.D("walkingBoxes true");
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

            isMoving = true;
            yield return StartCoroutine("IMoveTo", jarPos);
            SetDirection(Direction.Left);
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

                    this.D("walkingJars false");
                    walkingJars = false;
                    this.D("walkingBoxes false");
                    walkingBoxes = false;
                }
                else
                {
                    yield return StartCoroutine("HarvestBox", boxQueue.Dequeue());
                }
            }

            isMoving = false;
        }
        else if (nextBox == null)
        {
            yield return StartCoroutine("INotifyNoBoxAvailable");
        }
    }

    public void NotifyNoBoxAvailable()
    {
        StartCoroutine("INotifyNoBoxAvailable");
    }

    IEnumerator INotifyNoBoxAvailable()
    {
        bubble.sprite = bubbleSprites[3];
        yield return new WaitForSeconds(1);
        bubble.sprite = bubbleSprites[0];
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
            this.D("Player : Harvest Box : boxQueue.Count : " + boxQueue.Count);
            if (jarQueue.Count > 0)
            {
                this.D("Player : Harvest Box : jarQueue.Count : " + jarQueue.Count);
                yield return StartCoroutine("WalkJarToBox", jarQueue.Dequeue());
            }
            this.D("Player : Harvest Box : Done");
            Game.player.SetDirection(Direction.Down);
            Game.player.SetIdle(true);

            walkingBoxes = false;
            this.D("walkingBoxes false");

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
        this.D("WalkToDesk called: " + Time.fixedTime, gameObject);
        if (!isMoving) 
        {
            StartCoroutine("IWalkToDesk");
        }
    }

    IEnumerator IWalkToDesk()
    {
        isMoving = true;

        yield return StartCoroutine("IMoveTo", startingPos);
        SetDirection(Direction.Down);

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
