using System.Collections;
using System.Collections.Generic;
using Goals;
using UnityEngine;

public class SurviveGoal : Goal
{
    public override bool CanRun() {
        return true;
    }
    
    public override int CalculatePriority() {
        //  TODO: Make this goal's priority a function of the agent's needs
        return 50;
    }
}
