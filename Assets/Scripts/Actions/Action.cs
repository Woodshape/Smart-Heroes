using System;
using System.Collections.Generic;
using DefaultNamespace;
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
        [SerializeField]
        private List<Goal> supportedGoals = new List<Goal>();

        public Agent agent;
        
        //  Sprite to be used in the UI
        public Sprite sprite;

        protected Goal linkedGoal;

        public float tickTime = 1f;
        protected float tickTimer;

        protected void Awake() {
            agent = GetComponentInParent<Agent>();
        }

        protected void Update() {
            if (this is IDecayable) {
                IDecayable decayable = (IDecayable) this;
                decayable.Decay();
            }
        }

        public abstract bool CanRun();

        public abstract int CalculateCost();

        public virtual List<Goal> GetSupportedGoals() {
            return supportedGoals;
        }
        
        public virtual void OnActionUpdate() {
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickTime) {
                OnActionTick();

                tickTimer = 0f;
            }
        }

        public virtual void OnActionTick() {
        }
        
        public virtual void OnActionActivated(Goal goal) {
            Debug.Log($"GOAP -> Activating action for goal: {this} [{goal}]");

            linkedGoal = goal;
            
            //  Reset tick timer
            tickTimer = 0f;
        }
        
        public virtual void OnActionDeactivated() {
            Debug.Log("GOAP -> Deactivating action: " + this);

            linkedGoal = null;
            
            //  Reset tick timer
            tickTimer = 0f;
        }
    }
}
