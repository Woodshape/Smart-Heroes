using System;
using System.Collections.Generic;
using Goals;
using UnityEngine;
using Random = System.Random;

namespace Actions {
    public class WanderingAction : Action {

        private GameObject wanderingPoint;

        public override bool CanRun() {
            return true;
        }
        
        public override int CalculateCost() {
            return 1;
        }

        public override void OnActionTick() {
            //  FIXME: Testing only
            agent.awarenessComponent.FindInRange("Character", 10f);
        }

        public override void OnActionActivated(Goal goal) {
            base.OnActionActivated(goal);
            
            agent.ReachedDestinationEvent += AgentOnReachedDestinationEvent;

            Wander();
        }
        
        private void Wander() {
            Vector3 randomRelativePosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-10, 11),
                transform.position.y + UnityEngine.Random.Range(-10, 11), 0);

            if (wanderingPoint == null) {
                GameObject temp = new GameObject("Wanderpoint " + gameObject.transform.parent.name);
                temp.transform.position = randomRelativePosition;

                wanderingPoint = temp;
                wanderingPoint.transform.parent = gameObject.transform;
            }
            else {
                wanderingPoint.transform.position = randomRelativePosition;
            }

            agent.SetTarget(wanderingPoint);
        }

        public override void OnActionDeactivated() {
            base.OnActionDeactivated();

            agent.ReachedDestinationEvent -= AgentOnReachedDestinationEvent;

            Destroy(wanderingPoint);
            wanderingPoint = null;
            agent.target = null;
        }
        
        private void AgentOnReachedDestinationEvent() {
            Debug.Log("GOAP -> Wandering point reached...");

            Destroy(wanderingPoint);
            wanderingPoint = null;
            
            Wander();
        }
    }
}
