using System.Collections;
using System.Collections.Generic;
using GOAP;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class GAgent : MonoBehaviour {
    
    public GameObject actionContainer;
    public GameObject combatContainer;

    public GPlanner planner;
    public Queue<GAction> actionQueue;
    public GAction currentAction;
    public Goal currentGoal;

    public List<GAction> actions = new List<GAction>();
    public Dictionary<Goal, int> goals = new Dictionary<Goal, int>();

    void Start() {
        Goal goal = new Goal("win", 1, false);
        goals.Add(goal, 1);

        //  Start agent with all it's actions ready
        if (actionContainer != null) {
            Debug.Log("GOAP -> Actions found: ", gameObject);
            foreach (GAction action in actionContainer.GetComponentsInChildren<GAction>()) {
                actions.Add(action);
                
                Debug.Log($"GOAP -> {action}", gameObject);
            }
        }
    }
    
    void Update() { }

    public class Goal {
        public Dictionary<string, int> goals;
        public bool remove;

        public Goal(string state, int value, bool remove) {
            goals = new Dictionary<string, int>();
            goals.Add(state, value);
            this.remove = remove;

            Debug.Log($"GOAP -> Added Goal: {state}:{value}");
        }
    }
}
