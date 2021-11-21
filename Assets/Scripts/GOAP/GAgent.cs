using System.Collections;
using System.Collections.Generic;
using GOAP;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class GAgent : MonoBehaviour {
    public Transform target;

    public GameObject brain;

    public List<GAction> actions = new List<GAction>();
    
    private Vector3 lastTargetPosition = Vector3.zero;
    
    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    public bool reachedEndOfPath;
    
    // private NavMeshAgent _agent;
    private IAstarAI _ai;
    private Seeker _seeker;
    
    private int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start() {
        // _agent = GetComponent<NavMeshAgent>();
        // _agent.updateRotation = false;
        // _agent.updateUpAxis = false;

        _seeker = GetComponent<Seeker>();
        _ai = GetComponent<IAstarAI>();

        Goal goal = new Goal("win", 1, false);

        if (brain != null) {
            Debug.Log("GOAP -> Actions found: ", gameObject);
            foreach (GAction action in brain.GetComponentsInChildren<GAction>()) {
                actions.Add(action);
                
                Debug.Log($"GOAP -> {action}", gameObject);
            }
        }
        
        //  A* AI
        // Vector3 position = transform.position;
        // _seeker.StartPath(position, new Vector3(position.x + Random.Range(-10, -5),  position.y + Random.Range(-10, 10), 0), OnPathComplete);
    }

    // Update is called once per frame
    void Update() {
        if (_ai == null) {
            return;
        }
        
        if (target != null) {
            _ai.destination = target.position;
        }

        if (Vector3.Distance(lastTargetPosition, target.position) > 1f) {
            _ai.SearchPath();

            Debug.Log("A* -> Target moved, updating pathfinding.");
            
            lastTargetPosition = target.position;
        }
        
        // if (path == null) {
        //     // We have no path to follow yet, so don't do anything
        //     return;
        // }
        //
        // // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // // We do this in a loop because many waypoints might be close to each other and we may reach
        // // several of them in the same frame.
        // reachedEndOfPath = false;
        // // The distance to the next waypoint in the path
        // float distanceToWaypoint;
        // while (true) {
        //     // If you want maximum performance you can check the squared distance instead to get rid of a
        //     // square root calculation. But that is outside the scope of this tutorial.
        //     distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        //     if (distanceToWaypoint < nextWaypointDistance) {
        //         // Check if there is another waypoint or if we have reached the end of the path
        //         if (currentWaypoint + 1 < path.vectorPath.Count) {
        //             currentWaypoint++;
        //         } else {
        //             // Set a status variable to indicate that the agent has reached the end of the path.
        //             // You can use this to trigger some special code if your game requires that.
        //             reachedEndOfPath = true;
        //             break;
        //         }
        //     } else {
        //         break;
        //     }
        // }
        //
        // // Slow down smoothly upon approaching the end of the path
        // // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        // var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;
        //
        // // Direction to the next waypoint
        // // Normalize it so that it has a length of 1 world unit
        // Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // // Multiply the direction by our desired speed to get a velocity
        // Vector3 velocity = dir * speed * speedFactor;
        //
        //
        // // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
        // transform.position += velocity * Time.deltaTime;
        //
        // if (reachedEndOfPath) {
        //     Vector3 position = transform.position;
        //     _seeker.StartPath(position, new Vector3(position.x + Random.Range(-10, 10),  position.y + Random.Range(-10, 10), 0), OnPathComplete);
        // }
    }
    
    // public void OnPathComplete (Path p) {
    //     if (!p.error) {
    //         path = p;
    //         // Reset the waypoint counter so that we start to move towards the first point in the path
    //         currentWaypoint = 0;
    //     }
    //     else {
    //         Debug.LogWarning("A* -> Path was calculated with an error: " + p.errorLog, gameObject);
    //     }
    // }
    
    public class Goal {
        public Dictionary<string, int> goals;
        public bool remove;

        public Goal(string state, int value, bool remove) {
            goals = new Dictionary<string, int>();
            goals.Add(state, value);
            this.remove = remove;

            Debug.Log($"GOAP -> Added Goal: {state}:{value}");
        }
    }
}
