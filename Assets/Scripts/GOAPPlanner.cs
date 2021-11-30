
using System.Collections.Generic;
using Actions;
using Goals;
using UnityEngine;

namespace DefaultNamespace {
    public class GOAPPlanner : MonoBehaviour {
        public GameObject goalContainer;
        public GameObject actionContainer;
        public GameObject combatContainer;
        
        private List<Goal> goals = new List<Goal>();
        private List<Action> actions = new List<Action>();

        public Goal currentGoal;
        public Action currentAction;

        private void Awake() {
            if (goalContainer) {
                goals = new List<Goal>(GetComponentsInChildren<Goal>());
            }
            else {
                Debug.LogWarning("No goal container on: " + this);
            }
            
            if (actionContainer) {
                actions = new List<Action>(GetComponentsInChildren<Action>());
            }
            else {
                Debug.LogWarning("No action container on: " + this);
            }
        }

        private void Update() {
            Goal bestGoal = null;
            Action bestAction = null;
            
            foreach (Goal goal in goals) {
                //  Tick goal
                goal.OnGoalTick();
                
                if (!goal.CanRun()) {
                    continue;
                }

                //  Find highest priority goal that can be activated
                if (!(bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority())) {
                    continue;
                }

                Action candidateAction = null;
                
                //  Find action supporting this goal with the lowest cost
                foreach (Action action in actions) {
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
                Debug.Log($"New goal and action: {bestGoal} [{bestAction}]");
                
                OnGoalChanged(bestGoal, bestAction);
                
                // currentGoal = bestGoal;
                // currentAction = bestAction;
                //
                // //  Deactivate new goal and action
                // if (currentGoal != null) {
                //     currentGoal.OnGoalActivated();
                // }
                //
                // if (currentAction != null) {
                //     currentAction.OnActionActivated();
                // }
                
            } //  No change in goal
            else if (currentGoal == bestGoal) {
                //  Action changed
                if (currentAction != bestAction) {
                    OnActionChanged(bestAction);
                }
            } // We found a better goal
            else if (currentGoal != bestGoal) {
                Debug.Log($"New goal found: {currentGoal} -> {bestGoal}");
                
                OnGoalChanged(bestGoal, bestAction);
            }
            
            //  Nothing changed
            if (currentAction != null) {
                currentAction.OnActionTick();
            }
        }
        
        private void OnActionChanged(Action action) {
            Debug.Log($"Action changed: {currentAction} -> {action}");
            
            currentAction.OnActionDeactivated();

            currentAction = action;

            currentAction.OnActionActivated(currentGoal);
        }

        void OnGoalChanged(Goal goal, Action action) {
            Debug.Log($"Goal changed: {currentGoal} -> {goal}");
            Debug.Log($"Action changed: {currentAction} -> {action}");
            
            //  Deactivate new goal and action
            if (currentGoal != null) {
                currentGoal.OnGoalDeactivated();
            }

            if (currentAction != null) {
                currentAction.OnActionDeactivated();
            }
            
            currentGoal = goal;
            currentAction = action;
            
            //  Activate new goal and action
            if (currentGoal != null) {
                currentGoal.OnGoalActivated(action);
            }

            if (currentAction != null) {
                currentAction.OnActionActivated(goal);
            }
        }
    }
}
