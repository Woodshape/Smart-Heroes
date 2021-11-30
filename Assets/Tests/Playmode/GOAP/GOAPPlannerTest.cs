using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GOAPPlannerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void GOAPPlannerTestScenario()
    {
        // Use the Assert class to test conditions
        GameObject go = new GameObject();
        GOAPPlanner planner = go.AddComponent<GOAPPlanner>();
        
        Assert.NotNull(planner);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GOAPPlannerTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
