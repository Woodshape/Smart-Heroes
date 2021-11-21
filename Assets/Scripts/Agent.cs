using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Agent : MonoBehaviour {
    public Transform target;
    
    private Vector3 lastTargetPosition = Vector3.zero;
    
    private IAstarAI _ai;
    private Seeker _seeker;

    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _ai = GetComponent<IAstarAI>();
    }
    
    void Update() {
        PathfindingBehaviour();
    }
    
    private void PathfindingBehaviour() {
        if (_ai == null || target == null) {
            return;
        }

        //  AStar AI
        if (Vector3.Distance(lastTargetPosition, target.position) > 1f) {
            _ai.destination = target.position;
            _ai.SearchPath();

            Debug.Log("A* -> Target moved, updating pathfinding.");

            lastTargetPosition = target.position;
        }
    }
}
