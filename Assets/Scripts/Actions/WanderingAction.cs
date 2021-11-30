using System;
using System.Collections.Generic;
using Goals;
using UnityEngine;
using Random = System.Random;

namespace Actions {
    public class WanderingAction : Action {

        private GameObject wanderingPoint;

        public override int CalculateCost() {
            return 1;
        }

        public override void OnActionTick() {
            
        }

        public override void OnActionActivated(Goal goal) {
            base.OnActionActivated(goal);
            
            _agent.ReachedDestinationEvent += AgentOnReachedDestinationEvent;

            Wander();
        }
        
        private void Wander() {
            Vector3 randomRelativePosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-10, 11),
                transform.position.y + UnityEngine.Random.Range(-10, 11), 0);

            if (wanderingPoint == null) {
                GameObject temp = new GameObject("Wanderpoint " + gameObject.transform.parent.name);
                temp.transform.position = randomRelativePosition;

                wanderingPoint = temp;
            }
            else {
                wanderingPoint.transform.position = randomRelativePosition;
            }

            _agent.SetTarget(wanderingPoint);
        }

        public override void OnActionDeactivated() {
            base.OnActionDeactivated();

            _agent.ReachedDestinationEvent -= AgentOnReachedDestinationEvent;

            Destroy(wanderingPoint);
            wanderingPoint = null;
            _agent.target = null;
        }
        
        private void AgentOnReachedDestinationEvent() {
            Debug.Log("Wandering point reached...");

            Destroy(wanderingPoint);
            wanderingPoint = null;
            
            Wander();
        }
    }
}
