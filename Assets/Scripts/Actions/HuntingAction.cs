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

        public override void OnActionTick() {
            float random = Random.Range(0f, 1f);
            if (random <= 0.25f) {
                Debug.Log("Hunt success");
                currentCost = 5f;
            }
        }

        public void Decay() {
            currentCost -= Time.deltaTime;
        }
    }
}
