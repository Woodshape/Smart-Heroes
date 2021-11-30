using System.Collections;
using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class Node {
    public Node parent;
    public float cost;
    public Dictionary<string, int> states;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<string, int> states, GAction action) {
        this.parent = parent;
        this.cost = cost;
        this.states = new Dictionary<string, int>(states);
        this.action = action;
    }
}

public class GPlanner
{
    public GameObject agent;
    
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goals, WorldStates states) {
        List<GAction> usableActions = new List<GAction>();
        
        // Of all the actions available find the ones that can be achieved.
        foreach (GAction action in actions) {
            if (!action.IsAchievable()) {
                Debug.Log("Action not achievable: " + action, agent);
                continue;
            }
            
            usableActions.Add(action);
        }

        // Create the first node in the graph
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0.0f, GWorld.Instance.GetWorld().GetStates(), null);

        // Pass the first node through to start branching out the graph of plans from
        bool success = BuildGraph(start, leaves, usableActions, goals);
        if (!success) {
            Debug.LogWarning("GOAP -> No plan", agent);
            return null;
        }

        // Of all the plans found, find the one that's cheapest to execute and use that
        Node cheapest = null;
        foreach (Node leaf in leaves) {
            if (cheapest == null) {
                cheapest = leaf;
            }else if (leaf.cost < cheapest.cost) {
                cheapest = leaf;
            }
        }
        
        List<GAction> plan = new List<GAction>();
        Node node = cheapest;
        
        //  Build a linked list backwards from the cheapest leaf of the tree to the base node
        while (node != null) {
            if (node.action != null) {
                plan.Insert(0, node.action);
            }

            //  We will then eventually break out of the loop because the base (i.e. start node) has no parent
            node = node.parent;
        }

        // Make a queue out of the actions represented by the nodes in the plan for the agent to work its way through
        Queue<GAction> result = new Queue<GAction>();
        foreach (GAction action in plan) {
            result.Enqueue(action);
        }

        Debug.Log("GOAP -> --- Plan found ---", agent);
        foreach (GAction res in result) {
            Debug.Log("GOAP -> " + res, agent);
        }

        return result;
    }
    
    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> actions, Dictionary<string, int> goals) {
        bool pathFound = false;

        foreach (GAction action in actions) {
            //  Check whether the action is achievable given it's parent state
            if (action.IsAchievableGiven(parent.states)) {
                //  Copy parent states (start node = WorldStates), so we add the WorldStates first
                Dictionary<string, int> currentStates = new Dictionary<string, int>(parent.states);

                foreach (KeyValuePair<string, int> effects in action.aftereffects) {
                    //  Add the effects of the current action to the dictionary
                    if (!currentStates.ContainsKey(effects.Key)) {
                        currentStates.Add(effects.Key, effects.Value);
                    }
                }

                //  Accumulate action cost and states in the next action
                Node node = new Node(parent, parent.cost + action.cost, currentStates, action);

                //  If the current state of the world after doing this node's action is the goal
                //  this plan will achieve that goal and will become the agent's plan
                if (GoalsAchieved(goals, currentStates)) {
                    leaves.Add(node);
                    pathFound = true;
                }
                else {
                    //  If no goal has been found, branch out to add other actions to the plan
                    //  Remove action from list of usable actions
                    List<GAction> subset = ActionSubset(actions, action);
                    //  Recursively call BuildGraph with smaller list of actions 
                    pathFound = BuildGraph(node, leaves, subset, goals);
                }
            }
        }
        
        return pathFound;
    }
    
    private List<GAction> ActionSubset(List<GAction> actions, GAction action) {
        List<GAction> subset = new List<GAction>();
        foreach (GAction a in actions) {
            if (!a.Equals(action)) {
                subset.Add(a);
            }
        }
        return subset;
    }

    private bool GoalsAchieved(Dictionary<string, int> goals, Dictionary<string, int> currentStates) {
        foreach (KeyValuePair<string, int> g in goals) {
            if (!currentStates.ContainsKey(g.Key)) {
                return false;
            }
        }
        return true;
    }
}
