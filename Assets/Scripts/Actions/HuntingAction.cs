using System;
using DefaultNamespace;
using Goals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actions {
    public class HuntingAction : Action {

        private float currentCost = 5f;

        private void Update() {
            currentCost -= Time.deltaTime;
        }

        public override bool CanRun() {
            return true;
        }
        
        public override int CalculateCost() {
            return Mathf.FloorToInt(currentCost);
        }

        public override void OnActionTick() {
            //  FIXME
            float random = Random.Range(0f, 1f);
            if (random <= 0.25f) {
                currentCost = 5f;
            }
        }
    }
}
