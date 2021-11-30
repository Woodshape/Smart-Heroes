using System;
using System.Collections.Generic;
using Goals;
using UnityEngine;

namespace Actions {
    public interface IAction {
        int CalculateCost();
    
        void OnActionTick();
        void OnActionActivated(Goal goal);
        void OnActionDeactivated();
    }
    
    public abstract class Action : MonoBehaviour, IAction {
        public List<Goal> supportedGoals = new List<Goal>();

        protected Agent _agent;

        protected Goal linkedGoal;
        

        public void Awake() {
            _agent = GetComponentInParent<Agent>();
        }

        public abstract int CalculateCost();

        public virtual List<Goal> GetSupportedGoals() {
            return supportedGoals;
        }

        public virtual void OnActionTick() {
            
        }
        
        public virtual void OnActionActivated(Goal goal) {
            Debug.Log($"Activating action for goal: {this} [{goal}]");

            linkedGoal = goal;
        }
        
        public virtual void OnActionDeactivated() {
            Debug.Log("Deactivating action: " + this);

            linkedGoal = null;
        }
    }
}
