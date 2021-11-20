using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [Serializable]
    public class WorldState {
        public string key;
        public int value;
    }
    
    public class WorldStates {
        private Dictionary<string, int> states;

        public WorldStates() {
            states = new Dictionary<string, int>();
        }

        public void AddState(string state, int value) {
            states.Add(state, value);

            Debug.Log("Added state: " + state);
        }

        public void RemoveState(string state) {
            if (states.ContainsKey(state)) {
                states.Remove(state);
                
                Debug.Log("Removed state: " + state);
            }
        }

        public void ModifyState(string state, int value, bool setValue = false) {
            if (states.ContainsKey(state)) {
                if (setValue) {
                    //  Set state to value
                    states[state] = value;
                }
                else {
                    //  Modify state by value
                    states[state] += value;
                }
                
                Debug.Log("GOAP -> " + (setValue ? "Set" : "Modified") + " WorldState: " + states[state]);

                //  Remove state if value is 0 or negative
                if (states[state] <= 0) {
                    RemoveState(state);
                }
            }
            else {
                //  Add state if it did not already exist
                AddState(state, value);
            }
        }

        public bool HasState(string state) {
            return states.ContainsKey(state);
        }

        public Dictionary<string, int> GetStates() {
            return states;
        }
    }
}
