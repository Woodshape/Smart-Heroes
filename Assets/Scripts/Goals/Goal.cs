using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = Actions.Action;

namespace Goals {
    public interface IGoal {
        bool CanRun();
        int CalculatePriority();
    
        void OnGoalTick();
        void OnGoalActivated(Action action);
        void OnGoalDeactivated();
    }

    public abstract class Goal : MonoBehaviour, IGoal {

        protected Agent _agent;

        protected Action linkedAction;

        private void Awake() {
            _agent = GetComponentInParent<Agent>();
        }

        private void Update() {
            OnGoalTick();
        }

        public abstract bool CanRun();

        public abstract int CalculatePriority();

        public virtual void OnGoalTick() { }

        public virtual void OnGoalActivated(Action action) {
            Debug.Log($"Activating goal with action: {this} [{action}]");

            linkedAction = action;
        }

        public virtual void OnGoalDeactivated() {
            Debug.Log("Deactivating goal: " + this);

            linkedAction = null;
        }
    }
}
