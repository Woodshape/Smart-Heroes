using Goals;
using UnityEngine;

namespace Goals {
    public class IdleGoal : Goal {
        [SerializeField]
        private int priority = 10;

        public override bool CanRun() {
            return true;
        }

        public override int CalculatePriority() {
            return priority;
        }
    }
}

