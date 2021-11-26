using System.Collections;
using System.Collections.Generic;
using GOAP;
using Pathfinding;
using UnityEngine;

public class WanderAction : GAction {

    public override bool PrePerform() {
        Vector3 randomRelativePosition = new Vector3(transform.position.x + Random.Range(-10, 11), transform.position.y + Random.Range(-10, 11), 0); 
        if (destinationGO == null) {
            GameObject temp = new GameObject("Wanderpoint " + gameObject.transform.parent.name);
            temp.transform.position = randomRelativePosition;
        
            destinationGO = temp;
            destinationGO.transform.parent = this.gameObject.transform;
        }
        else {
            destinationGO.transform.position = randomRelativePosition;
        }
        

        duration = Random.Range(1f, 60f);

        StartCoroutine(nameof(WanderDuration));

        return true;
    }

    IEnumerator WanderDuration() {
        yield return new WaitForSeconds(Random.Range(1, 11));

        // Destroy(destinationGO);

        isRunning = false;
    }

    public override bool PostPerform() {
        // Destroy(destinationGO);
        
        return true;
    }
}
