using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using MushroomBox.Debug;

public class John : Character
{
    public GameObject[] enterPathNodes;
    public GameObject[] exitPathNodes;
    public Tutorial tutorial;
    public GameObject walkableArea;
    private TextDialog _textDialog;
    public GameObject shedArrow;

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

    public void WalkIn()
    {
        StartCoroutine("IWalkIn");
    }

    public void Exit()
    {
        StartCoroutine("WalkExitPath");
    }

    IEnumerator WalkEnterPath()
	{
        walkableArea.SetActive(false);
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

    IEnumerator IWalkIn()
    {
        walkableArea.SetActive(false);
        transform.position = new Vector2(Screen.width + 400, enterPathNodes[0].transform.position.y);
        yield return StartCoroutine("MoveLeftTo", enterPathNodes[2].transform.position);
        yield return StartCoroutine("MoveUpTo", enterPathNodes[4].transform.position);
        yield return StartCoroutine("MoveLeftTo", enterPathNodes[4].transform.position);
        GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>().text = "";
        _textDialog.GetComponent<TextDisplayEffect>().texts = new string[3] {"Thanks!", "You could grow more if you use the shed next door.", "I'll clean it out for you for some gas money."};
        _textDialog.GetComponent<TextDisplayEffect>().currentPage = 0;
        _textDialog.ShowDialog();
        Game.player.SetDirection(Direction.Right);
        yield return StartCoroutine("ShowDialog2");
    }

    IEnumerator ShowDialog()
    {
        yield return new WaitForSeconds(0.5f);

        if (_textDialog.isActive == false)
        {
            GameObject.Find("Goals Popup").GetComponent<Animator>().SetTrigger("Extend");
            //CustomEvent.Trigger(this.gameObject, "John Done");
            Game.player.SetDirection(Direction.Down);
            Game.counter.coinChange(100, true);
            Game.counter.sporeChange(10, true);
            SaveSystem.SaveGame();
            StartCoroutine("WalkExitPath");
            yield return new WaitForSeconds(3f);
            tutorial.StartTutorial();

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

    IEnumerator ShowDialog2()
    {
        yield return new WaitForSeconds(0.5f);

        if (_textDialog.isActive == false)
        {
            GameObject goalsPopup = GameObject.Find("Goals Popup");
            goalsPopup.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Clean out the shed";
            goalsPopup.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "0 / 20";
            goalsPopup.GetComponent<Animator>().SetTrigger("Extend");
            Game.goalDisplays[0].gameObject.SetActive(true);
            Game.goalDisplays[1].gameObject.SetActive(true);
            Game.goalDisplays[0].GetComponentsInChildren<TextMeshProUGUI>()[1].text = Game.goals[1].Minimum + " / " + Game.goals[1].Maximum;
            Game.goal = 1;
            this.D("goal displays: " + Game.goalDisplays[0].GetComponentsInChildren<TextMeshProUGUI>()[1].text);
	        // exit
            Game.player.SetDirection(Direction.Down);
            Game.counter.mushroomChange(-5);
            Game.pauseCustomers = false;
            shedArrow.SetActive(true);
            SaveSystem.SaveGame();
            StartCoroutine("IWalkExit");
            Game.walkableArea.gameObject.SetActive(true);
            if (Game.player.boxQueue.Count > 0)
                Game.player.WalkToBoxes();
            yield break;
        }

        if (_textDialog.eventFlag == true)
        {
            yield return StartCoroutine("ShowPlayerBubble");
            _textDialog.eventFlag = false;
            _textDialog.StartDialog();
        }

        StartCoroutine("ShowDialog2");
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
        isMoving = false;
    }

    IEnumerator IWalkExit()
	{
		this.D("TestA");
        yield return StartCoroutine("MoveDownTo", exitPathNodes[0].transform.position);
        yield return StartCoroutine("MoveRightTo", new Vector3(Screen.width + 400, enterPathNodes[0].transform.position.y));
        transform.position = enterPathNodes[4].transform.position;
		isMoving = false;
		this.D("Exited...");
    }
}
