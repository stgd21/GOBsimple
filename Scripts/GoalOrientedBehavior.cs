using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalOrientedBehavior : MonoBehaviour
{
    Goal[] initialGoals;
    Action[] detailedActionArray;

    private int numberOfTimeChanges = 0;
    public float timeIncrementAmount = 1f;

    private void Start()
    {
        Behave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoAction();
        }
    }

    void DoAction()
    {
        //Get action to match most pressing goal
        Action bestChoice = chooseAction(detailedActionArray, initialGoals);
        Debug.Log("The best option is " + bestChoice.name);
        for (int i = 0; i < initialGoals.Length; i++)
        {
            initialGoals[i].value += bestChoice.getGoalChange(initialGoals[i]);
        }
        PrintGoalStatus();
    }

    Action chooseAction(Action[] actions, Goal[] goals)
    {
        //Find action leading to lowest discontentment
        Action bestAction = null;
        float bestValue = Mathf.Infinity;

        foreach (Action action in actions)
        {
            float thisValue = discontentment(action, goals);
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }
        return bestAction;
    }

    float discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0;

        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.getGoalChange(goal);

            newValue += action.getDuration() * goal.getChange();
            discontentment += goal.getDiscontentment(newValue);
        }
        return discontentment;
    }

    void Behave()
    {
        //Set up inital goals to work toward
        initialGoals = new Goal[3];
        //Fill this array with new goals
        initialGoals[0] = new Goal("eat", 6, 2);
        initialGoals[1] = new Goal("sleep", 5, 1.75f);
        initialGoals[2] = new Goal("bathroom", 4, 2.5f);

        //Define my available actions
        Action eatSnackAction = new Action("Eat a snack", 3f);
        Action sleepInBedAction = new Action("Sleep in bed", 5f);
        Action goToBathroomAction = new Action("Go to bathroom", 4f);

        //Fill array with actions
        detailedActionArray = new Action[3];
        //Initialize Actions
        detailedActionArray[0] = eatSnackAction;
        detailedActionArray[1] = sleepInBedAction;
        detailedActionArray[2] = goToBathroomAction;
        //Set up related goals and weights
        //Snack relations
        detailedActionArray[0].goals.Add(new Goal("eat", -3f, 1));
        detailedActionArray[0].goals.Add(new Goal("sleep", 1f, 0.5f));
        detailedActionArray[0].goals.Add(new Goal("bathroom", 2f, 2.5f));
        //Sleep relations
        detailedActionArray[1].goals.Add(new Goal("sleep", -4f, 0.5f));
        //Bathroom relations
        detailedActionArray[2].goals.Add(new Goal("bathroom", -3f, 2.5f));
        detailedActionArray[2].goals.Add(new Goal("eat", 1f, 1));

        PrintGoalStatus();

        //Now represent the passing of time with a function that repeats every second
        InvokeRepeating("PassTime", 0f, timeIncrementAmount);
    }

    void PassTime()
    {
        //Go through the initial goals and add to their value to update their importance
        for (int i = 0; i < initialGoals.Length; i++)
        {
            //updates value internally
            initialGoals[i].change();
        }
        numberOfTimeChanges++;
        Debug.Log("Time Has Passed for the " + numberOfTimeChanges + " time");
    }

    void PrintGoalStatus()
    {
        Debug.Log("Goals Update: ");
        for (int i = 0; i < initialGoals.Length; i++)
        {
            Debug.Log(initialGoals[i].name + " has an urgency value of: " + initialGoals[i].value);
        }
    }
}
