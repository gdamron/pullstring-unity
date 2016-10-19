#if UNIT_TEST

using System.Threading;
using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("Schedule Timer")]
public class ScheduleTimer : TestBase
{
    protected override void Run(Response response, int step)
    {
        switch (step)
        {
            case 0:
                conversation.SendActivity("timer");
                break;
            case 1:
                TestUtils.TextShouldMatch(response, new[] { "Starting timer" });
                conversation.SendText("intervening input");
                break;
            case 2:
                TestUtils.TextShouldMatch(response, new[] { "Ignored" });
                Thread.Sleep(2100);
                conversation.CheckForTimedResponse();
                break;
            case 3:
                TestUtils.TextShouldMatch(response, new[] { "Timer fired" }, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif