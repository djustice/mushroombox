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

    IEnumerator WalkUpTo(Vector3 dest)
    {
        SetDirection(Direction.Up);
        SetIdle(false);

        if ((int)transform.position.y == (int)dest.y)
        {
            SetIdle(true);
            yield break;
        }

        Vector2 origin = transform.position;
        Vector2 destination = new Vector2(origin.x, dest.y);
        accel = 0;
        while ((int)transform.position.y != (int)dest.y)
        {
            transform.position = Vector2.Lerp(origin, destination, accel);
            accel += speed * Time.deltaTime;
            yield return null;
        }

        SetIdle(true);
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

    IEnumerator WalkDownTo(Vector3 dest)
    {
        SetDirection(Direction.Down);
        SetIdle(false);

        Vector2 origin = transform.position;
        Vector2 destination = new Vector2(origin.x, dest.y);

        accel = 0;
        while ((int)transform.position.y != (int)dest.y)
        {
            transform.position = Vector2.Lerp(origin, destination, accel);
            accel += speed * Time.deltaTime;
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

    IEnumerator WalkLeftTo(Vector3 dest)
    {
        SetDirection(Direction.Left);
        SetIdle(false);

        Vector2 origin = transform.position;
        Vector2 destination = new Vector2(dest.x, origin.y);

        if ((int)transform.position.x == (int)dest.x)
        {
            SetDirection(Direction.Left);
            SetIdle(true);
            yield break;
        }

        accel = 0;
        while ((int)transform.position.x != (int)dest.x)
        {
            transform.position = Vector2.Lerp(origin, destination, accel);
            accel += speed * Time.deltaTime;
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

    IEnumerator WalkRightTo(Vector3 dest)
    {
        SetIdle(false);
        SetDirection(Direction.Right);

        Vector2 origin = transform.position;
        Vector2 destination = new Vector2(dest.x, origin.y);

        accel = 0;
        while ((int)transform.position.x != (int)dest.x)
        {
            transform.position = Vector2.Lerp(origin, destination, accel);
            accel += speed * Time.deltaTime;
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

    IEnumerator WalkTo(Vector3 dest)
    {
        int xDistance;
        int yDistance;

        if ((int)transform.position.x > (int)dest.x)
        {
            xDistance = (int)transform.position.x - (int)dest.x;
        } else
        {
            xDistance = (int)dest.x - (int)transform.position.x;
        }

        if ((int)transform.position.y > (int)dest.y)
        {
            yDistance = (int)transform.position.y - (int)dest.y;
        } else
        {
            yDistance = (int)dest.y - (int)transform.position.y;
        }

        if (xDistance >= yDistance)
        {
            if ((int)transform.position.x >= (int)dest.x)
            {
                yield return StartCoroutine("WalkLeftTo", dest);
                if ((int)transform.position.y >= (int)dest.y)
                {
                    yield return StartCoroutine("WalkDownTo", dest);
                }
                else
                {
                    yield return StartCoroutine("WalkUpTo", dest);
                }
            } else
            {
                yield return StartCoroutine("WalkRightTo", dest);
                if ((int)transform.position.y >= (int)dest.y)
                {
                    yield return StartCoroutine("WalkDownTo", dest);
                }
                else
                {
                    yield return StartCoroutine("WalkUpTo", dest);
                }
            }
        } else
        {
            if ((int)transform.position.y >= (int)dest.y)
            {
                yield return StartCoroutine("WalkDownTo", dest);
                if ((int)transform.position.x >= (int)dest.x)
                {
                    yield return StartCoroutine("WalkLeftTo", dest);
                    SetDirection(Direction.Left);
                } else
                {
                    yield return StartCoroutine("WalkRightTo", dest);
                    SetDirection(Direction.Right);
                }
            } else
            {
                yield return StartCoroutine("WalkUpTo", dest);
                if ((int)transform.position.x >= (int)dest.x)
                {
                    yield return StartCoroutine("WalkLeftTo", dest);
                    SetDirection(Direction.Left);
                }
                else
                {
                    yield return StartCoroutine("WalkRightTo", dest);
                    SetDirection(Direction.Right);
                }
            }
        }

        SetDirection(Direction.Left);
        SetIdle(true);
    }

    IEnumerator MoveTo(Vector3 dest)
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
                }
                else
                {
                    yield return StartCoroutine("MoveUpTo", dest);
                }
            }
            else
            {
                yield return StartCoroutine("MoveRightTo", dest);
                if ((int)transform.position.y >= (int)dest.y)
                {
                    yield return StartCoroutine("MoveDownTo", dest);
                }
                else
                {
                    yield return StartCoroutine("MoveUpTo", dest);
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

        SetDirection(Direction.Left);
        SetIdle(true);
    }
}
