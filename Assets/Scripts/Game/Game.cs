using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

using MushroomBox.Debug;

public class Game : MonoBehaviour
{
    public Customer customer;
    public CollectibleCounter collectibleCounter;
    public static Player player;
	public static John john;
	public Customer[] customers;
    public static bool pauseCustomers = true;
    public int previousCustomer = 0;
    public static CollectibleCounter counter;
    public static int coinCount = 0;
    public static int coinCountTotal = 0;
    public static int mushroomCount = 0;
    public static int mushroomCountTotal = 0;
    public static int sporeCount = 0;
    public static int sporeCountTotal = 0;
    public static int jarCount = 1;
    public static Placeholder jarPlaceholder;
    public static List<Jar> jars;
	public static int boxCount = 1;
	public static Placeholder boxPlaceholder;
	public static List<Box> boxes;
	public static int sporeRewardFactor = 5;

	public GameObject[] uiButtons;
	public static GameObject shopButton;
	public static GameObject goalsButton;
	public static GameObject settingsButton;

    public GameObject[] uiDialogs;
    public static GameObject shopDialog;
    public static GameObject goalsDialog;
    public static GameObject settingsDialog;

    public GameObject shedArrow;
    public GameObject houseArrow;

    public static WalkableArea walkableArea;

    public Tutorial tutorial;
    
    public static List<Goal> goals;
    public static List<GoalDisplay> goalDisplays;
    public static int goal;

    public static bool[] shedItems = new bool[16];
    public GameObject[] shedItem;
	
	public bool doIntro = true;
	public bool skipToL2 = true;

    void Start()
    {
        MushroomBoxDebug.GetLoggerByType<Customer>().logger.filterLogType = LogType.Error;
        MushroomBoxDebug.GetLoggerByType<WalkableArea>().logger.filterLogType = LogType.Error;

        player = FindObjectOfType<Player>();
        john = FindObjectOfType<John>();
        walkableArea = FindObjectOfType<WalkableArea>();

        Placeholder[] placeholders = FindObjectsOfType<Placeholder>();
        foreach (Placeholder placeholder in placeholders)
        {
            if (placeholder.item == 1)
                jarPlaceholder = placeholder;

            if (placeholder.item == 2)
                boxPlaceholder = placeholder;
        }

        counter = collectibleCounter;

        jars = new List<Jar>();
        Game.jars.Add(FindObjectOfType<Jar>());

        boxes = new List<Box>();
        Game.boxes.Add(FindObjectOfType<Box>());
        
        shopButton = uiButtons[0];
        goalsButton = uiButtons[1];
        settingsButton = uiButtons[2];

        shopDialog = uiDialogs[0];
        goalsDialog = uiDialogs[1];
        settingsDialog = uiDialogs[2];

        goals = new List<Goal>();
        goalDisplays = new List<GoalDisplay>();
        foreach (GoalDisplay g in FindObjectsOfType<GoalDisplay>())
        {
            goals.Add(g.goal);
            goalDisplays.Add(g);
            this.D(g.goal.Text);
        }

        for (int i = 0; i < shedItems.Length; i++)
        {
            shedItems[i] = true;
        }

        if (File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            GameData loadData = SaveSystem.LoadGame();
            Game.counter.coinChange(loadData.coinCount, true);
            Game.coinCountTotal = loadData.coinCountTotal;
            Game.counter.mushroomChange(loadData.mushroomCount, true);
            Game.mushroomCountTotal = loadData.mushroomCountTotal;
            Game.counter.sporeChange(loadData.sporeCount, true);
            Game.sporeCountTotal = loadData.sporeCountTotal;

            for (int i = 1; i < loadData.jarCount; i++)
            {
                Vector3 pos = Game.jarPlaceholder.transform.position;
                Game.jarPlaceholder.transform.position = new Vector3(pos.x, pos.y - 180);
                Jar newJar = Instantiate<Jar>(FindObjectOfType<Jar>(), Game.jars[0].transform.parent, false);
                newJar.transform.position = pos;
                Game.jars.Add(newJar);

                if (jars.Count == 3)
                {
                    jarPlaceholder.gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < loadData.jarCount; i++)
            {
                if (loadData.jarProgresses[i] == 8)
                {
                    jars[i].SetProgress(7);
                } else
                {
                    jars[i].SetProgress(loadData.jarProgresses[i]);
                }

                jars[i].SetState(loadData.jarStates[i], true);
            }

            for (int i = 1; i < loadData.boxCount; i++)
            {
                Vector3 pos = boxPlaceholder.transform.position;
                boxPlaceholder.transform.position = new Vector3(pos.x + 185, pos.y);
                Box newBox = Instantiate<Box>(FindObjectOfType<Box>(), boxes[0].transform.parent, false);
                newBox.transform.position = pos;
                boxes.Add(newBox);

                if (boxes.Count == 4)
                {
                    boxPlaceholder.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < loadData.boxCount; i++)
            {
                boxes[i].SetProgress(loadData.boxProgresses[i]);
                boxes[i].SetState(loadData.boxStates[i], true);
            }

            for (int i = 0; i < loadData.goalCount; i++)
            {
                goals[i].Value = loadData.goalValues[i];
                goals[i].Maximum = loadData.goalMaximums[i];
                goals[i].Complete = loadData.goalCompletions[i];
                goal = i;
            }

            goal = loadData.currentGoal;
            if (goal == 0)
            {
                goalDisplays[0].gameObject.SetActive(false);
            }

            Game.pauseCustomers = loadData.pauseCustomers;
            if (pauseCustomers == false)
            {
                shedArrow.SetActive(true);
                this.D("ShedArrowButton true");
            }

            for (int i = 0; i < loadData.shedItems.Length - 1; i++)
            {
                this.D("Load: shed item " + i + " " + loadData.shedItems[i]);
                shedItem[i].SetActive(loadData.shedItems[i]);
                shedItems[i] = loadData.shedItems[i];
            }

            SaveSystem.SaveGame();
        }
        else
        {
            if (doIntro == true) {
                john.Enter();
                shopButton.SetActive(false);
                goalsButton.SetActive(false);
                settingsButton.SetActive(false);
                goalDisplays[0].gameObject.SetActive(false);
                mushroomCountTotal = 0;
                coinCountTotal = 0;
                sporeCountTotal = 0;
                GameObject.Find("Camera").GetComponent<Animator>().SetTrigger("PanIn");
            }
        }
        
        StartCoroutine("CustomerLoop");
    }

	IEnumerator WaitForIntro() 
	{
		yield return new WaitForSeconds(8);
		uiButtons[0].SetActive(true);
		uiButtons[1].SetActive(true);
		uiButtons[2].SetActive(true);
	}

	IEnumerator CustomerLoop() 
	{
		yield return new WaitForSeconds(5f);
		
        if (pauseCustomers == false)
        {
            int x = (int)Random.Range(0, customers.Length);

            while (x == previousCustomer)
            {
                x = (int)Random.Range(0, customers.Length);
            }

            if (mushroomCount > 0)
            {
                bool foundMover = false;
                foreach (Customer c in customers)
                {
                    if (c.isMoving)
                        foundMover = true;
                }

                if (!foundMover)
                {
                    previousCustomer = x;
                    customers[x].StartCheckForMushrooms();
                }
            }
        }
		
		StartCoroutine("CustomerLoop");
	}
	
    public static int getJarCount()
    {
        return jarCount;
    }

    public static Box getNextAvailBox()
    {
        foreach(Box box in boxes)
        {
            if (box.state == BoxState.Empty)
            {
                return box;
            }
        }

        return null;
    }

    public static void EnableL2()
    {
        GameObject.Find("WalkableArea").SetActive(false);
        GameObject.Find("Goals Popup Progress").GetComponent<TextMeshProUGUI>().text = "5 / 5";
        GameObject.Find("Goals Popup").GetComponent<Animator>().SetTrigger("Extend");
        Game.player.WalkToDesk();
        Game.john.WalkIn();
    }
}
