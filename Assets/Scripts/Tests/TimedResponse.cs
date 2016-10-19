#if UNIT_TEST

using System.Threading;
using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("Timed Response")]
public class TimedResponse : TestBase
{
    protected override void Run(Response response, int step)
    {
        switch (step)
        {
            case 0:
                conversation.SendActivity("fafa5f56-d6f1-4381-aec8-ce37a68e465f");
                break;
            case 1:
                TestUtils.TextShouldMatch(response, new[] { "Say something" });
                conversation.CheckForTimedResponse();
                break;
            case 2:
                if (response.Outputs != null && response.Outputs.Length > 0)
                {
                    IntegrationTest.Fail("Should not have recieved output when checking for timed response");
                }
                Thread.Sleep(2100);
                conversation.CheckForTimedResponse();
                break;
            case 3:
                TestUtils.TextShouldMatch(response, new[] { "I'm waiting" });
                conversation.SendText("hit the fallback");
                break;
            case 4:
                TestUtils.TextShouldMatch(response, new[] { "That was something", "Yes it was" }, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif