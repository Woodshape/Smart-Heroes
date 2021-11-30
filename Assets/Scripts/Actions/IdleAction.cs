using System;
using System.Collections;
using System.Collections.Generic;
using Goals;
using UnityEngine;
using Action = Actions.Action;

public class IdleAction : Action {
    public override bool CanRun() {
        return true;
    }
    
    public override int CalculateCost() {
        return 10;
    }
}
