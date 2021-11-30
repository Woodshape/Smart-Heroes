using Actions;
using UnityEngine;

namespace Goals {
    public class WanderGoal : Goal {
        [SerializeField]
        private int minPriority = 0;
        [SerializeField]
        private int maxPriority = 30;

        [SerializeField]
        private float priorityBuildRate = 1f;
        [SerializeField]
        private float priorityDecayRate = 0.1f;
        private float currentPriority = 0f;
        
        public override bool CanRun() {
            return true;
        }
        
        public override int CalculatePriority() {
            return Mathf.FloorToInt(currentPriority);
        }

        public override void OnGoalTick() {
            if (_agent.IsMoving()) {
                currentPriority -= priorityDecayRate * Time.deltaTime;
            }
            else {
                currentPriority += priorityBuildRate * Time.deltaTime;
            }

            currentPriority = Mathf.Clamp(currentPriority, minPriority, maxPriority);
        }

        public override void OnGoalActivated(Action action) {
            base.OnGoalActivated(action);
            
            currentPriority = maxPriority;
        }
    }
}
