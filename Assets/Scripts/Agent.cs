using System;
using System.Collections;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour {
    public GameObject target;
    
    private Vector3 lastTargetPosition = Vector3.zero;

    private GAgent _agent;
    private GOAPPlanner _planner;
    private IAstarAI _ai;

    public event Action ReachedDestinationEvent;

    private void Awake() {
        _agent = GetComponent<GAgent>();
        _planner = GetComponent<GOAPPlanner>();
        _ai = GetComponent<IAstarAI>();
        
        OnCreate();
    }

    void LateUpdate() {
        PathfindingBehaviour();
    }

    public void SetTarget(GameObject newTarget) {
        string tar = this.target != null ? this.target.transform.position.ToString() : "No target";
        Debug.Log($"A* -> Setting new target: {tar} -> {newTarget.transform.position}");
        this.target = newTarget;

        if (target != null) {
            Debug.Log("A* -> Setting new destination: " + target.transform.position);
            _ai.destination = target.transform.position;
            if (!_ai.pathPending) {
                Debug.Log("A* -> Searching new path");
                _ai.SearchPath();
            }
        }
    }

    public bool IsMoving() {
        if (!_ai.hasPath) {
            return false;
        }
        
        return !(_ai.reachedEndOfPath && !_ai.pathPending);
    }
    
    private void PathfindingBehaviour() {
        if (_ai == null || target == null) {
            // Debug.LogWarning("A* -> Agent or target unknown...");
            return;
        }

        //  AStar AI
        // if (!destinationInvoked && _ai.reachedDestination) {
        if (_ai.reachedEndOfPath && !_ai.pathPending) {
            Debug.Log("A* -> Agent at destination", gameObject);

            ReachedDestinationEvent?.Invoke();
        }
    }
    
    private void OnActionChanged() {
        if (_agent.currentAction != null) {
            Debug.Log("A* -> Action changed, setting new target", gameObject);
            
            SetTarget(_agent.currentAction.destinationGO);
            // SetTarget(_planner.currentAction.target);
        }
    }

    private void OnCreate() {
        // GetComponent<GAgent>().ActionChangedEvent += OnActionChanged;
        // GetComponent<GOAPPlanner>().ActionChangedEvent += OnActionChanged;
    }
    
    private void OnDestroy() {
        // GetComponent<GAgent>().ActionChangedEvent -= OnActionChanged;
    }
}
