using System.Collections;
using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class HuntAction : GAction
{
    public override bool PrePerform() {
        Vector3 randomRelativePosition = new Vector3(transform.position.x + Random.Range(-10, 11), transform.position.y + Random.Range(-10, 11), 0); 
        if (destinationGO == null) {
            GameObject temp = new GameObject("Hunt Point " + gameObject.transform.parent.name);
            temp.transform.position = randomRelativePosition; 
        
            destinationGO = temp;
            destinationGO.transform.parent = this.gameObject.transform;
        }
        else {
            destinationGO.transform.position = randomRelativePosition;
        }
        
        return true;
    }
    public override bool PostPerform() {
        // Destroy(destinationGO);
        
        return true;
    }
}
