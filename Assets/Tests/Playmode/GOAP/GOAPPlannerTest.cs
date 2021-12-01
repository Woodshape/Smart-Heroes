using System.Collections.Generic;
using System.Runtime.Serialization;
using Actions;
using DefaultNamespace;
using Goals;
using NSubstitute;
using NUnit.Framework;

public class GOAPPlannerTest
{

    [Test]
    public void GOAPPlannerTestScenario()
    {
        WanderGoal wanderGoal = Substitute.For<WanderGoal>();
        WanderingAction wanderAction = Substitute.For<WanderingAction>();

        GOAPPlanner planner = Substitute.For<GOAPPlanner>();

        List<Goal> goals = new List<Goal> {wanderGoal};
        List<Action> actions = new List<Action> {wanderAction};

        GOAPPlanner _planner = (GOAPPlanner) FormatterServices.GetUninitializedObject(typeof(GOAPPlanner));
        Assert.NotNull(_planner);

        wanderAction.CanRun().Returns(true);
        wanderAction.CalculateCost().Returns(1);
        wanderAction.GetSupportedGoals().Returns(goals);

        wanderGoal.CanRun().Returns(true);
        wanderGoal.CalculatePriority().Returns(10);

        planner.GetGoals.Returns(goals);
        planner.GetActions.Returns(actions);

        Assert.AreEqual(goals, planner.GetGoals);
        Assert.AreEqual(actions, planner.GetActions);

        for (int i = 0; i < 11; i++) {
            planner.Update();
        }
    }
}
