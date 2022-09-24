using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class John : Character
{
    public GameObject[] enterPathNodes;
    public GameObject[] exitPathNodes;
    private TextDialog _textDialog;

    public void Start()
    {
        startingPos = transform.position;
        isMoving = false;
        speed = 350f;
        _textDialog = FindObjectOfType<TextDialog>();
    }

    public void Enter()
    {
        StartCoroutine("WalkEnterPath");
    }

    public void Exit()
    {
        StartCoroutine("WalkExitPath");
    }

    IEnumerator WalkEnterPath()
	{
		transform.position = new Vector2(Screen.width + 400, enterPathNodes[0].transform.position.y);
        yield return StartCoroutine("MoveLeftTo", enterPathNodes[0].transform.position);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine("MoveUpTo", enterPathNodes[0].transform.position);
        yield return new WaitForSeconds(0.8f);
        yield return StartCoroutine("MoveDownTo", enterPathNodes[1].transform.position);
        yield return StartCoroutine("MoveRightTo", enterPathNodes[2].transform.position);
        yield return StartCoroutine("MoveUpTo", enterPathNodes[3].transform.position);
        yield return new WaitForSeconds(0.15f);
        yield return StartCoroutine("MoveLeftTo", enterPathNodes[3].transform.position);
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine("MoveRightTo", enterPathNodes[3].transform.position);
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine("MoveUpTo", enterPathNodes[4].transform.position);
        yield return StartCoroutine("MoveLeftTo", enterPathNodes[4].transform.position);
        _textDialog.ShowDialog();
        Game.player.SetDirection(Direction.Right);
        yield return StartCoroutine("ShowDialog");
    }

    IEnumerator ShowDialog()
    {
        yield return new WaitForSeconds(0.5f);

        if (_textDialog.isActive == false)
        {
            GameObject.Find("Goals Popup").GetComponent<Animator>().SetTrigger("Extend");
            CustomEvent.Trigger(this.gameObject, "John Done");
            StartCoroutine("WalkExitPath");
            Game.player.SetDirection(Direction.Down);
            Game.counter.coinChange(100, true);
            Game.counter.sporeChange(10, true);
            SaveSystem.SaveGame();
            yield break;
        }

        if (_textDialog.eventFlag == true)
        {
            yield return StartCoroutine("ShowPlayerBubble");
            _textDialog.eventFlag = false;
            _textDialog.StartDialog();
        }

        StartCoroutine("ShowDialog");
    }

    IEnumerator ShowPlayerBubble()
    {
        Game.player.bubble.sprite = Game.player.bubbleSprites[2];
        yield return new WaitForSeconds(1f);
        Game.player.bubble.sprite = Game.player.bubbleSprites[0];
    }

    IEnumerator WalkExitPath()
    {
        yield return StartCoroutine("MoveDownTo", exitPathNodes[0].transform.position);
        yield return StartCoroutine("MoveRightTo", exitPathNodes[1].transform.position);
        yield return new WaitForSeconds(0.8f);
        yield return StartCoroutine("MoveLeftTo", exitPathNodes[2].transform.position);
        transform.position = startingPos;
        isMoving = false;
    }
}
