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

        void OnGoalUpdate();
        void OnGoalTick();
        void OnGoalActivated(Action action);
        void OnGoalDeactivated();
    }

    public abstract class Goal : MonoBehaviour, IGoal {
        protected Agent _agent;

        protected Action linkedAction;
        
        protected float tickTime = 1f;
        protected float tickTimer;

        protected void Awake() {
            _agent = GetComponentInParent<Agent>();
        }

        public abstract bool CanRun();

        public abstract int CalculatePriority();

        public virtual void OnGoalUpdate() {
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickTime) {
                OnGoalTick();

                tickTimer = 0f;
            }
        }

        public virtual void OnGoalTick() { }

        public virtual void OnGoalActivated(Action action) {
            Debug.Log($"GOAP -> Activating goal with action: {this} [{action}]");

            linkedAction = action;
            
            //  Reset tick timer
            tickTimer = 0f;
        }

        public virtual void OnGoalDeactivated() {
            Debug.Log("GOAP -> Deactivating goal: " + this);

            linkedAction = null;
            
            //  Reset tick timer
            tickTimer = 0f;
        }
    }
}
