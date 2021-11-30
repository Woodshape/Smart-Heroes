using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
        public List<Action> updateActions = new List<Action>();

        protected Agent _agent;

        protected Action linkedAction;

        private void Awake() {
            _agent = GetComponentInParent<Agent>();
        }

        private void Update() {
            OnGoalTick();

            if (this is IDecayable) {
                IDecayable decayable = (IDecayable) this;
                decayable.Decay();
            }

            //  Tick other actions that need to be ticked passively
            // foreach (Action action in updateActions) {
            //     if (action != linkedAction) {
            //         action.OnActionTick();
            //     }
            // }
        }

        public abstract bool CanRun();

        public abstract int CalculatePriority();

        public virtual void OnGoalTick() { }

        public virtual void OnGoalActivated(Action action) {
            Debug.Log($"GOAP -> Activating goal with action: {this} [{action}]");

            linkedAction = action;
        }

        public virtual void OnGoalDeactivated() {
            Debug.Log("GOAP -> Deactivating goal: " + this);

            linkedAction = null;
        }
    }
}
