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

        void OnActiveTick();
        void OnGoalTick();
        void OnGoalActivated(Action action);
        void OnGoalDeactivated();
    }

    public abstract class Goal : MonoBehaviour, IGoal {
        protected Agent _agent;

        protected Action linkedAction;

        protected void Awake() {
            _agent = GetComponentInParent<Agent>();
        }

        protected void Update() {
            OnGoalTick();

            if (this is IDecayable) {
                IDecayable decayable = (IDecayable) this;
                decayable.Decay();
            }
        }

        public abstract bool CanRun();

        public abstract int CalculatePriority();
        
        public virtual void OnActiveTick() { }

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
