using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP {
    public abstract class GAction : MonoBehaviour {
        // Name of the action
        public string actionName;
        // Cost of the action
        // This value could also be used to evaluate the "emotion" of the agent performing this action - i.e. higher cost means the agent would rather do something else
        public float cost = 1.0f;
        //  Sprite to be used in the UI
        public Sprite sprite;
        // Target where the action is going to take place
        public GameObject destinationGO = null;
        // Store the tag
        public string destinationTag;
        // Duration the action should take
        public float duration = 0.0f;
        
        // An array of WorldStates of preconditions
        [SerializeField]
        private WorldState[] preConditions;
        // An array of WorldStates of afterEffects
        [SerializeField]
        private WorldState[] afterEffects;

        [SerializeReference]
        public Dictionary<string, int> preconditions = new Dictionary<string, int>();
        [SerializeReference]
        public Dictionary<string, int> aftereffects = new Dictionary<string, int>();

        public WorldStates beliefs;
        
        //  Are we currently performing an action?
        public bool isRunning = false;

        public void Awake() {
            foreach (WorldState state in preConditions) {
                preconditions.Add(state.key, state.value);
            }
            
            foreach (WorldState state in afterEffects) {
                aftereffects.Add(state.key, state.value);
            }
        }
        
        public abstract bool PrePerform();
        
        public abstract bool PostPerform();

        public virtual bool IsAchievable() {
            return true;
        }

        public bool IsAchievableGiven(Dictionary<string, int> conditions) {
            if (preconditions.Count == 0) {
                return true;
            }
            
            //  If any given condition is not part of the action's preconditions, the action is not achievable
            //  viz. all given conditions must be met by the action's preconditions
            foreach (KeyValuePair<string, int> condition in preconditions) {
                if (!conditions.ContainsKey(condition.Key)) {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() {
            return $"{base.ToString()}, {nameof(actionName)}: {actionName}, {nameof(cost)}: {cost}, {nameof(duration)}: {duration}, {nameof(preconditions)}: {String.Join(" | ", preconditions)}, {nameof(aftereffects)}: {String.Join(" | ", aftereffects)}";
        }
    }
}
