using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour {
    public GameObject target;
    
    private Vector3 lastTargetPosition = Vector3.zero;

    private GAgent _agent;
    private IAstarAI _ai;

    public event Action ReachedDestinationEvent;

    private void Awake() {
        _agent = GetComponent<GAgent>();
        _ai = GetComponent<IAstarAI>();
        
        OnCreate();
    }

    void LateUpdate() {
        PathfindingBehaviour();
    }

    private bool destinationInvoked;

    public void SetTarget(GameObject target) {
        this.target = target;
        
        destinationInvoked = false;
        
        if (target != null) {
            _ai.destination = target.transform.position;
            if (!_ai.pathPending) {
                _ai.SearchPath();
            }
        }
    }
    
    private void PathfindingBehaviour() {
        if (_ai == null || target == null) {
            return;
        }

        //  AStar AI
        if (!destinationInvoked && _ai.reachedDestination) {
            Debug.Log("A* -> Agent at destination", gameObject);
            
            ReachedDestinationEvent?.Invoke();

            destinationInvoked = true;
        }
    }
    
    private void OnActionChanged() {
        if (_agent.currentAction != null) {
            Debug.Log("A* -> Action changed, setting new target");
            
            SetTarget(_agent.currentAction.destinationGO);
        }
    }

    private void OnCreate() {
        GetComponent<GAgent>().ActionChangedEvent += OnActionChanged;
    }
    
    private void OnDestroy() {
        GetComponent<GAgent>().ActionChangedEvent -= OnActionChanged;
    }
}
