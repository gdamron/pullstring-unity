#if UNIT_TEST

using System.Collections.Generic;
using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("Events and Behaviors")]
public class EventsAndBehaviors : TestBase
{

    protected override void Run(Response response, int step)
    {
        switch (step)
        {
            case 0:
                conversation.SendEvent("simple_event", null);
                break;
            case 1:
                var expected = new BehaviorOutput()
                {
                    Behavior = "simple_action",
                    Parameters = null
                };

                Dictionary<string, object> parameters = new Dictionary<string, object> {
                    {"name", "green"}
                };
                TestUtils.BehaviorShouldMatch(expected, response);
                conversation.SendEvent("event_with_param", parameters);
                break;
            case 2:
                var expected2 = new BehaviorOutput()
                {
                    Behavior = "action_with_param",
                    Parameters = new Dictionary<string, ParameterValue> {
                        {"name", new ParameterValue("Green")}
                    }
                };
                Dictionary<string, object> parameters2 = new Dictionary<string, object> {
                    {"name", "red"}
                };
                TestUtils.TextShouldMatch(response, new[] { "Green Event Called" });
                TestUtils.BehaviorShouldMatch(expected2, response);
                conversation.SendEvent("event_with_param", parameters2);
                break;
            case 3:
                var expected3 = new BehaviorOutput()
                {
                    Behavior = "action_with_param",
                    Parameters = new Dictionary<string, ParameterValue> {
                        {"name", new ParameterValue("Red")}
                    }
                };
                TestUtils.TextShouldMatch(response, new[] { "Red Event Called" });
                TestUtils.BehaviorShouldMatch(expected3, response, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif