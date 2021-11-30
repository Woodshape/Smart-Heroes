using DefaultNamespace;
using Goals;
using UnityEngine;

namespace Actions {
    public class HuntingAction : Action, IDecayable {

        private float currentCost = 5f;
        
        public override bool CanRun() {
            return true;
        }
        
        public override int CalculateCost() {
            return Mathf.FloorToInt(currentCost);
        }

        public override void OnActionActivated(Goal goal) {
            base.OnActionActivated(goal);

            //  FIXME
            currentCost = 5f;
        }
        
        public void Decay() {
            currentCost -= Time.deltaTime;
        }
    }
}
