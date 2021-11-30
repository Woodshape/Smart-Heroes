using GOAP;
using UnityEngine;

public class RandomAction : GAction {
    [Range(0f, 1.0f)]
    public float chance;
    
    public override bool PrePerform() {
        float random = Random.Range(0f, 1f);
        return chance > random;
    }
    
    public override bool PostPerform() {
        return true;
    }
}
