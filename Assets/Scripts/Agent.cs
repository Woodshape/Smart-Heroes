using System;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour {
    public Transform target;
    
    private Vector3 lastTargetPosition = Vector3.zero;
    
    private IAstarAI _ai;

    public event Action ReachedDestination;

    void Start()
    {
        _ai = GetComponent<IAstarAI>();
        
        GameObject tar = Instantiate(new GameObject("Target_" + gameObject.name), transform.position, Quaternion.identity);
        target = tar.transform;
        target.position = new Vector3(lastTargetPosition.x + Random.Range(-10, 10), lastTargetPosition.y + Random.Range(-10, 10), 0);
    }
    
    void Update() {
        PathfindingBehaviour();
    }

    private bool destinationInvoked;
    
    private void PathfindingBehaviour() {
        if (_ai == null || target == null) {
            return;
        }

        //  AStar AI
        if (!destinationInvoked && _ai.reachedDestination) {
            Debug.Log("A* -> Agent at destination", gameObject);
            
            ReachedDestination?.Invoke();

            destinationInvoked = true;
        }

        Vector3 targetPosition = target.position;
        if (Vector3.Distance(lastTargetPosition, targetPosition) > 1f) {
            _ai.destination = targetPosition;
            _ai.SearchPath();
        
            Debug.Log("A* -> Updating pathfinding");
        
            lastTargetPosition = targetPosition;
        
            destinationInvoked = false;
        }
    }
}
