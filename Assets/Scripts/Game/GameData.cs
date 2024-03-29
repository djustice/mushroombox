// default to 0 for old savefile versions

public class GameData
{
    public int coinCount = 0;
    public int coinCountTotal = 0;
    public int mushroomCount = 0;
    public int mushroomCountTotal = 0;
    public int sporeCount = 0;
    public int sporeCountTotal = 0;
    public int jarCount = 0;
    public int[] jarProgresses = new int[0];
    public int[] jarStates = new int[0];
    public int boxCount = 0;
    public int[] boxProgresses = new int[0];
    public int[] boxStates = new int[0];
    public int goalCount = 0;
    public int[] goalValues = new int[0];
    public int[] goalMaximums = new int[0];
    public bool[] goalCompletions = new bool[0];
    public int currentGoal = 0;
    public bool[] shedItems = new bool[18];
    public bool pauseCustomers = true;

    public GameData()
    {
        coinCount = Game.coinCount;
        coinCountTotal = Game.coinCountTotal;
        mushroomCount = Game.mushroomCount;
        mushroomCountTotal = Game.mushroomCountTotal;
        sporeCount = Game.sporeCount;
        sporeCountTotal = Game.sporeCountTotal;
        
        jarCount = Game.jars.Count;
        jarProgresses = new int[jarCount];
        int jpi = 0;
        while (jpi < jarCount)
        {
            jarProgresses[jpi] = Game.jars[jpi].progress;
            jpi++;
        }
        jarStates = new int[jarCount];
        int jsi = 0;
        while (jsi < jarCount)
        {
            jarStates[jsi] = (int)Game.jars[jsi].state;
            jsi++;
        }
        boxCount = Game.boxes.Count;
        boxProgresses = new int[boxCount];
        int bpi = 0;
        while (bpi < boxCount)
        {
            boxProgresses[bpi] = Game.boxes[bpi].progress;
            bpi++;
        }
        boxStates = new int[boxCount];
        int bsi = 0;
        while (bsi < boxCount)
        {
            boxStates[bsi] = (int)Game.boxes[bsi].state;
            bsi++;
        }

        goalCount = Game.goals.Count;
        currentGoal = Game.goal;
        goalValues = new int[goalCount];
        goalMaximums = new int[goalCount];
        goalCompletions = new bool [goalCount];
        int goalIndex = 0;
        while (goalIndex < goalCount)
        {
            goalValues[goalIndex] = (int)Game.goals[goalIndex].Value;
            goalMaximums[goalIndex] = (int)Game.goals[goalIndex].Maximum;
            goalCompletions[goalIndex] = Game.goals[goalIndex].Complete;
            goalIndex++;
        }

        shedItems = Game.shedItems;
        int shedItemIndex = 0;
        while (shedItemIndex < shedItems.Length)
        {
            shedItems[shedItemIndex] = Game.shedItems[shedItemIndex];
            shedItemIndex++;
        }

        pauseCustomers = Game.pauseCustomers;
    }
}
