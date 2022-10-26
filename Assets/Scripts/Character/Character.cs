using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer bubble;
    public Sprite[] bubbleSprites;
    public Animator animator;

    public bool isMoving;

    public Vector3 startingPos;

    public float accel;
    public float speed;

    public void SetIdle(bool idle)
    {
        animator.SetBool("Idle", idle);
    }

    public void SetDirection(int direction)
    {
        animator.SetInteger("Direction", direction);
    }

    public int GetDirection()
    {
        return animator.GetInteger("Direction");
    }
    
    IEnumerator MoveUpTo(Vector3 dest)
    {
        SetDirection(Direction.Up);
        SetIdle(false);

        if ((int)transform.position.y == (int)dest.y)
        {
            SetIdle(true);
            yield break;
        }

        Vector3 origin = transform.position;
        Vector3 destination = new Vector3(origin.x, dest.y);
        accel = 0;
        while ((int)transform.position.y != (int)dest.y)
        {
            transform.position = Vector3.MoveTowards(origin, destination, accel);
            accel += speed * Time.deltaTime;

	        int diff = (int)transform.position.y - (int)dest.y;
            if (diff >= -1 && diff <= 1)
            {
            	SetIdle(true);
	            yield break;
            }

            yield return null;
        }

        SetIdle(true);
    }

    IEnumerator MoveDownTo(Vector3 dest)
    {
        SetDirection(Direction.Down);
        SetIdle(false);

        Vector3 origin = transform.position;
        Vector3 destination = new Vector3(origin.x, dest.y);

        accel = 0;
        while ((int)transform.position.y != (int)dest.y)
        {
            transform.position = Vector3.MoveTowards(origin, destination, accel);
            accel += speed * Time.deltaTime;

	        int diff = (int)transform.position.y - (int)dest.y;
            if (diff >= -1 && diff <= 1)
            {
	            SetIdle(true);
                yield break;
            }

            yield return null;
        }

        SetIdle(true);
    }

    IEnumerator MoveLeftTo(Vector3 dest)
    {
        SetDirection(Direction.Left);
        SetIdle(false);

        Vector3 origin = transform.position;
        Vector3 destination = new Vector3(dest.x, origin.y, 0);

        if ((int)transform.position.x == (int)dest.x)
        {
            SetDirection(Direction.Left);
            SetIdle(true);
            yield break;
        }

        accel = 0;
        while ((int)transform.position.x != (int)dest.x)
        {
            transform.position = Vector3.MoveTowards(origin, destination, accel);
            accel += speed * Time.deltaTime;

            int diff = (int)transform.position.x - (int)dest.x;
            if (diff >= -1 && diff <= 1)
            {
            	SetIdle(true);
                yield break;
            }

            yield return null;
        }

        SetIdle(true);
    }

    IEnumerator MoveRightTo(Vector3 dest)
    {
        SetIdle(false);
        SetDirection(Direction.Right);

        Vector3 origin = transform.position;
        Vector3 destination = new Vector3(dest.x, origin.y);

        accel = 0;
        while ((int)transform.position.x != (int)dest.x)
        {
            transform.position = Vector3.MoveTowards(origin, destination, accel);
	        accel += speed * Time.deltaTime;

	        int diff = (int)transform.position.x - (int)dest.x;
	        if (diff >= -1 && diff <= 1) 
	        {
		        SetIdle(true);
	        	yield break;
	        }

	        yield return null;
        }

        SetIdle(true);
    }

    public void WalkToThenStop(Vector3 dest)
    {
        StartCoroutine("IWalkToThenStop", dest);
    }

    IEnumerator IWalkToThenStop(Vector3 dest)
    {
        yield return StartCoroutine("IMoveTo", dest);
        SetDirection(Direction.Down);
    }

    public void MoveTo(Vector3 dest)
    {
        StartCoroutine("IMoveTo", dest);
    }

    IEnumerator IMoveTo(Vector3 dest)
    {
        int xDistance;
        int yDistance;

        if ((int)transform.position.x > (int)dest.x)
        {
            xDistance = (int)transform.position.x - (int)dest.x;
        }
        else
        {
            xDistance = (int)dest.x - (int)transform.position.x;
        }

        if ((int)transform.position.y > (int)dest.y)
        {
            yDistance = (int)transform.position.y - (int)dest.y;
        }
        else
        {
            yDistance = (int)dest.y - (int)transform.position.y;
        }

        if (xDistance >= yDistance)
        {
            if ((int)transform.position.x >= (int)dest.x)
            {
                yield return StartCoroutine("MoveLeftTo", dest);
                if ((int)transform.position.y >= (int)dest.y)
                {
                    yield return StartCoroutine("MoveDownTo", dest);
                    SetDirection(Direction.Down);
                }
                else
                {
                    yield return StartCoroutine("MoveUpTo", dest);
                    SetDirection(Direction.Up);
                }
            }
            else
            {
                yield return StartCoroutine("MoveRightTo", dest);
                if ((int)transform.position.y >= (int)dest.y)
                {
                    yield return StartCoroutine("MoveDownTo", dest);
                    SetDirection(Direction.Down);
                }
                else
                {
                    yield return StartCoroutine("MoveUpTo", dest);
                    SetDirection(Direction.Up);
                }
            }
        }
        else
        {
            if ((int)transform.position.y >= (int)dest.y)
            {
                yield return StartCoroutine("MoveDownTo", dest);
                if ((int)transform.position.x >= (int)dest.x)
                {
                    yield return StartCoroutine("MoveLeftTo", dest);
                    SetDirection(Direction.Left);
                }
                else
                {
                    yield return StartCoroutine("MoveRightTo", dest);
                    SetDirection(Direction.Right);
                }
            }
            else
            {
                yield return StartCoroutine("MoveUpTo", dest);
                if ((int)transform.position.x >= (int)dest.x)
                {
                    yield return StartCoroutine("MoveLeftTo", dest);
                    SetDirection(Direction.Left);
                }
                else
                {
                    yield return StartCoroutine("MoveRightTo", dest);
                    SetDirection(Direction.Right);
                }
            }
        }

        SetIdle(true);
    }
}
