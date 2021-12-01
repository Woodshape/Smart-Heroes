
using System.Collections.Generic;
using Actions;
using Goals;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class GOAPPlanner : MonoBehaviour {
        public GameObject goalContainer;
        public GameObject actionContainer;
        public GameObject combatContainer;
        
        public ActionIconHandler actionIconManager;

        public GameObject debugUIContainer;
        
        private List<Goal> goals = new List<Goal>();
        private List<Action> actions = new List<Action>();

        public virtual List<Goal> GetGoals => goals;
        public virtual List<Action> GetActions => actions;

        public Goal currentGoal;
        public Action currentAction;

        private void Awake() {
            if (goalContainer) {
                goals = new List<Goal>(GetComponentsInChildren<Goal>());
            }
            else {
                Debug.LogWarning("GOAP -> No goal container on: " + this);
            }
            
            if (actionContainer) {
                actions = new List<Action>(GetComponentsInChildren<Action>());
            }
            else {
                Debug.LogWarning("GOAP -> No action container on: " + this);
            }
        }

        public void Update() {
            Goal bestGoal = null;
            Action bestAction = null;
            
            foreach (Goal goal in GetGoals) {
                //  Update and tick goal
                goal.OnGoalUpdate();
                
                if (!goal.CanRun()) {
                    continue;
                }

                //  Find highest priority goal that can be activated
                if (!(bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority())) {
                    continue;
                }

                Action candidateAction = null;
                
                //  Find action supporting this goal with the lowest cost
                foreach (Action action in GetActions) {
                    if (!action.CanRun()) {
                        continue;
                    }
                    
                    if (!action.GetSupportedGoals().Contains(goal)) {
                        continue;
                    }

                    if (!(candidateAction == null || action.CalculateCost() < candidateAction.CalculateCost())) {
                        continue;
                    }

                    candidateAction = action;
                }
                
                //  Did we find an action?
                if (candidateAction != null) {
                    bestAction = candidateAction;
                    bestGoal = goal;
                }
            }

            //  Did we find a goal?
            if (currentGoal == null) {
                Debug.Log($"GOAP -> New goal and action: {bestGoal} [{bestAction}]");
                
                OnGoalChanged(bestGoal, bestAction);

            } //  No change in goal
            else if (currentGoal == bestGoal) {
                //  Action changed
                if (currentAction != bestAction) {
                    OnActionChanged(bestAction);
                }
            } // We found a better goal
            else if (currentGoal != bestGoal) {
                Debug.Log($"GOAP -> New goal found: {currentGoal} -> {bestGoal}");
                
                OnGoalChanged(bestGoal, bestAction);
            }
            
            //  Nothing changed, simply update our current action
            if (currentAction != null) {
                currentAction.OnActionUpdate();
            }
        }

        public void AddGoal(Goal goalToAdd) {
            
        }
        
        public void AddGoals(List<Goal> goalsToAdd) {
            
        }

        public void RemoveGoal(Goal goalToRemove) {
            
        }
        
        public void RemoveGoals(List<Goal> goalsToRemove) {
            
        }

        public void AddAction(Action actionToAdd) {
            
        }
        
        public void AddActions(List<Action> actionsToAdd) {
            
        }

        public void RemoveAction(Action actionToRemove) {
            if (!actions.Contains(actionToRemove))
                return;

            //  First, check if the action we want to remove is currently active
            if (currentAction.Equals(actionToRemove)) {
                currentAction.OnActionDeactivated();
            }

            actions.Remove(actionToRemove);
        }
        
        public void RemoveActions(List<Action> actionsToRemove) {
            foreach (Action action in actionsToRemove) {
                RemoveAction(action);
            }
        }
        
        private void OnActionChanged(Action action) {
            Debug.Log($"GOAP -> Action changed: {currentAction} -> {action}");
            
            currentAction.OnActionDeactivated();

            currentAction = action;

            currentAction.OnActionActivated(currentGoal);
            
            DisplayDebugUI();
        }

        private void OnGoalChanged(Goal goal, Action action) {
            Debug.Log($"GOAP -> Goal changed: {currentGoal} -> {goal}");
            Debug.Log($"GOAP -> Action changed: {currentAction} -> {action}");
            
            //  Deactivate new goal and action
            StopGoal();
            StopAction();
            
            currentGoal = goal;
            currentAction = action;
            
            //  Activate new goal and action
            StartGoal(action);
            StartAction(goal);

            DisplayDebugUI();
        }

        private void StartAction(Goal goal) {
            if (currentAction == null) {
                return;
            }
            
            if (actionIconManager != null && currentAction.sprite != null) {
                StartCoroutine(actionIconManager.Spawn(currentAction.sprite));
            }
            
            currentAction.OnActionActivated(goal);
        }
        
        private void StopAction() {
            if (currentAction == null) {
                return;
            }
            
            currentAction.OnActionDeactivated();
        }

        private void StartGoal(Action action) {
            if (currentGoal == null) {
                return;
            }

            currentGoal.OnGoalActivated(action);
        }

        private void StopGoal() {
            if (currentGoal == null) {
                return;
            }
            
            currentGoal.OnGoalDeactivated();
        }

        private void DisplayDebugUI() {
            if (debugUIContainer != null) {
                debugUIContainer.GetComponent<TextMeshProUGUI>().text = $"{currentGoal} -> {currentAction}";
            }
        }
    }
}
