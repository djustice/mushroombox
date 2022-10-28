using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Customer customer;
    public CollectibleCounter collectibleCounter;
    public static Player player;
	public static John john;
	public Customer[] customers;
    public bool pauseCustomers = true;
    public static CollectibleCounter counter;
    public static int coinCount = 0;
    public static int mushroomCount = 0;
    public static int sporeCount = 0;
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

    public static WalkableArea walkableArea;

    public Tutorial tutorial;
    
    public static List<Goal> goals;
    public static int goal;
	
	public bool doIntro = true;
	public bool skipToL2 = true;

    void Start()
    {
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

        // goals = new List<Goal>();
        // foreach (GoalDisplay g in FindObjectsOfType<GoalDisplay>())
        // {
        //     goals.Add(g.goal);
        //     Debug.Log(g.goal.Text);
        // }

        if (File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            GameData loadData = SaveSystem.LoadGame();
            Game.counter.coinChange(loadData.coinCount, true);
            Game.counter.mushroomChange(loadData.mushroomCount, true);
            Game.counter.sporeChange(loadData.sporeCount, true);

            for (int i = 1; i < loadData.jarCount; i++)
            {
                Vector3 pos = Game.jarPlaceholder.transform.position;
	            Game.jarPlaceholder.transform.position = new Vector3(pos.x, pos.y - 180);
                Jar newJar = Instantiate<Jar>(FindObjectOfType<Jar>(), Game.jars[0].transform.parent, false);
                newJar.transform.position = pos;
                Game.jars.Add(newJar);

                if (Game.jars.Count == 3)
                {
                    Game.jarPlaceholder.gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < loadData.jarCount; i++)
            {
                if (loadData.jarProgresses[i] == 8)
                {
                    Game.jars[i].SetProgress(7);
                } else
                {
                    Game.jars[i].SetProgress(loadData.jarProgresses[i]);
                }

                Game.jars[i].SetState(loadData.jarStates[i], true);
            }

            for (int i = 1; i < loadData.boxCount; i++)
            {
                Vector3 pos = Game.boxPlaceholder.transform.position;
	            Game.boxPlaceholder.transform.position = new Vector3(pos.x + 185, pos.y);
                Box newBox = Instantiate<Box>(FindObjectOfType<Box>(), Game.boxes[0].transform.parent, false);
                newBox.transform.position = pos;
                Game.boxes.Add(newBox);

                if (Game.boxes.Count == 4)
                {
                    Game.boxPlaceholder.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < loadData.boxCount; i++)
            {
                Game.boxes[i].SetProgress(loadData.boxProgresses[i]);
                Game.boxes[i].SetState(loadData.boxStates[i], true);
            }

            // for (int i = 0; i < loadData.goalCount; i++)
            // {
            //     Game.goals[i].Value = loadData.goalValues[i];
            //     Game.goals[i].Maximum = loadData.goalMaximums[i];
            //     goal = i;
            // }

            SaveSystem.SaveGame();
        }
        else
        {
        	if (doIntro == true) {
	        	john.Enter();
	        	shopButton.SetActive(false);
	        	goalsButton.SetActive(false);
	        	settingsButton.SetActive(false);
	        	GameObject.Find("Camera").GetComponent<Animator>().SetTrigger("PanIn");
	        	
        	}
        }
        
	    // if (skipToL2 == true)
	    // {
		//    GameObject.Find("Camera").GetComponent<Animator>().SetTrigger("PanLevel2");
		//    Input.gyro.enabled = true;
	    // }
        
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
		
		int x = (int) Random.Range(0, customers.Length);
		
		if (mushroomCount > 0)
		{
			bool foundMover = false;
			foreach(Customer c in customers) 
			{
				if (c.isMoving)
					foundMover = true;
			}
			
			if (!foundMover)
				customers[x].StartCheckForMushrooms();
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
}
