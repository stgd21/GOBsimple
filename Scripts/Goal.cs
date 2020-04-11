using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{

    public string name;
    public float value;
    //As implemented, changeRate for initial goals is the increment amount for change based on time,
    //But for goals in an Action's list, changeRate is used as the amounts applied to the initial goals
    public float changeRate;

    public float getDiscontentment(float newValue)
    {
        return newValue * newValue;
    }

    public float getChange()
    {
        //Return amount of change that the goal normally experiences per unit of time (seconds here)
        return changeRate;
    }

    public Goal(string newName, float newValue, float newChangeRate = 0f)
    {
        name = newName;
        value = newValue;
        changeRate = newChangeRate;
    }

    public void change()
    {
        //Called every time increment
        value += changeRate;
    }
}

public class Action
{
    public string name; //For debug output
    public List<Goal> goals;
    //duration is total duration, no other operations will be done
    float duration;

    public float getGoalChange(Goal goal)
    {
        //returns change in insistence that carrying out the action would provide
        for (int i = 0; i < goals.Count; i++)
        {
            //Find the right one
            if (goal.name == goals[i].name)
            {
                return goals[i].value;
            }
        }
        return 0;
    }

    public float getDuration()
    {
        return duration;
    }

    public Action(string newName, float newDuration, List<Goal> newGoals = null)
    {
        name = newName;
        duration = newDuration;

        if (goals != null)
        {
            goals = newGoals;
        }
        else
        {
            goals = new List<Goal>();
        }
    }
}
