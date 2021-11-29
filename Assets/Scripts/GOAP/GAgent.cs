using System;
using System.Collections.Generic;
using System.Linq;
using GOAP;
using UnityEngine;

public class GAgent : MonoBehaviour {
    
    public GameObject actionContainer;
    public GameObject combatContainer;

    public ActionIconHandler actionIconManager;

    public Goal currentGoal;
    public GAction currentAction;
    
    private GPlanner planner;
    private Queue<GAction> actionQueue;

    private List<GAction> actions = new List<GAction>();
    private Dictionary<Goal, int> goals = new Dictionary<Goal, int>();
    
    private bool invoked;
    private bool atDestination;

    public event Action ActionChangedEvent;

    void Awake() {
        OnCreate();
    }

    void Start() {
        GameObject go = this.gameObject;
        
        Goal wanderGoal = new Goal("wander", 1, false, go);
        Goal huntGoal = new Goal("hunt", 1, false, go);

        goals.Add(wanderGoal, 3);
        goals.Add(huntGoal, 1);

        //  Start agent with all it's actions ready
        if (actionContainer != null) {
            Debug.Log("GOAP -> --- Actions found ---", gameObject);
            foreach (GAction action in actionContainer.GetComponentsInChildren<GAction>()) {
                actions.Add(action);
                
                Debug.Log($"GOAP -> {action}", gameObject);
            }
        }
    }

    void LateUpdate() {
        //  1) Check if we currently have an action that is running and if so, do nothing else
        if (CheckCurrentAction())
            return;
        
        //  2) If we don't have a plan or a running action, get a new plan and action queue
        CheckNewPlan();
        
        //  3) Check if we have an empty action queue (viz. we've run out of things to do)
        CheckEmptyActionQueue();

        //  4) We still have actions to do
        CheckActionQueue();
    }

    private void OnReachedDestination() {
        // Debug.Log("Registered agent at destination", gameObject);
        atDestination = true;
    }

    private bool CheckCurrentAction() {
        if (currentAction != null && currentAction.isRunning) {
            //  Check if the agent has reached it's destination
            if (atDestination) {
                if (!invoked) {
                    Invoke(nameof(CompleteAction), currentAction.duration);
                    invoked = true;
                }
            }

            //  If we have an action, don't create a new plan or action
            return true;
        }

        return false;
    }

    private void CheckNewPlan() {
        if (planner == null || actionQueue == null) {
            //  We have no plan to work on, so create one
            planner = new GPlanner();
            planner.agent = this.gameObject;

            //  Sort our goals and subgoals based on their value
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach (KeyValuePair<Goal, int> subGoals in sortedGoals) {
                actionQueue = planner.Plan(actions, subGoals.Key.goals, null);
                if (actionQueue != null) {
                    currentGoal = subGoals.Key;
                    
                    Debug.Log("GOAP -> --- Current goals ---", gameObject);
                    foreach (KeyValuePair<string, int> goal in currentGoal.goals) {
                        Debug.Log($"GOAP -> {goal}", gameObject);
                    }
                    
                    break;
                }
            }
        }
    }

    private void CheckEmptyActionQueue() {
        if (actionQueue != null && actionQueue.Count == 0) {
            //  Do we need to remove the current goal?
            if (currentGoal.remove) {
                goals.Remove(currentGoal);
            }

            // Set planner = null so it will trigger a new one in step 2
            planner = null;
        }
    }
    
    private void CheckActionQueue() {
        if (actionQueue != null && actionQueue.Count > 0) {
            //  Remove the top action of the queue and put it in currentAction
            currentAction = actionQueue.Dequeue();

            //  Check if all conditions to perform the action are met
            CheckAction();
            
            //  Are we stuck?
            if (actionQueue != null && currentAction != null && !currentAction.isRunning) {
                Debug.LogWarning("GOAP -> Agent stuck... Calculating new plan.", gameObject);
                planner = null;
            }
        }
    }
    
    private void CheckAction() {
        if (currentAction.PrePerform()) {
            //  Check if our action has a target or a target tag assigned to it
            DetermineActionTarget();

            if (currentAction.destinationGO != null) {
                //  We do have a target, so start the action
                StartAction();
            }
            
            ActionChangedEvent?.Invoke();
        }
        else {
            // Our action cannot be performed yet, so force a new plan in step 2
            actionQueue = null;
        }
    }

    private void DetermineActionTarget() {
        //  We don't have a target but rather a target tag assigned
        if (currentAction.destinationGO == null && currentAction.destinationTag != "") {
            //  Find a gameObject corresponding to that tag
            currentAction.destinationGO = GameObject.FindWithTag(currentAction.destinationTag);
        }
    }
    
    private void StartAction() {
        Debug.Log("GOAP -> Action started: " + currentAction, gameObject);

        currentAction.isRunning = true;

        if (actionIconManager != null && currentAction.sprite != null) {
            StartCoroutine(actionIconManager.Spawn(currentAction.sprite));
        }
    }

    private void CompleteAction() {
        Debug.Log("GOAP -> Action completed: " + currentAction, gameObject);
        
        currentAction.isRunning = false;
        currentAction.PostPerform();
        
        invoked = false;
        atDestination = false;
    }
    
    #region Events

    private void OnCreate() {
        GetComponent<Agent>().ReachedDestinationEvent += OnReachedDestination;
    }
    
    private void OnDestroy() {
        GetComponent<Agent>().ReachedDestinationEvent -= OnReachedDestination;
    }

    #endregion
    

    [Serializable]
    public class Goal {
        [SerializeReference]
        public Dictionary<string, int> goals;
        public bool remove;

        public Goal(string state, int value, bool remove) {
            goals = new Dictionary<string, int>();
            goals.Add(state, value);
            this.remove = remove;

            Debug.Log($"GOAP -> Added Goal: {state}:{value}");
        }
        
        public Goal(string state, int value, bool remove, GameObject context) {
            goals = new Dictionary<string, int>();
            goals.Add(state, value);
            this.remove = remove;

            Debug.Log($"GOAP -> Added Goal: {state}:{value}", context);
        }
    }
}
