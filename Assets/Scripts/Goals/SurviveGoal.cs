using System.Collections;
using System.Collections.Generic;
using Actions;
using DefaultNamespace;
using Goals;
using UnityEngine;

public class SurviveGoal : Goal
{
    private float currentPriority = 0f;

    private bool decay;
    
    public override bool CanRun() {
        return true;
    }
    
    public override int CalculatePriority() {
        //  TODO: Make this goal's priority a function of the agent's needs
        return Mathf.FloorToInt(currentPriority);
    }

    public override void OnGoalTick() {
        if (!decay) {
            currentPriority += Time.deltaTime;
        }
        else {
            currentPriority -= Time.deltaTime;
        }

        if (currentPriority <= 0f) {
            decay = false;
        }
        if (currentPriority >= 20f) {
            decay = true;
        }
    }
}
