using System.Threading;
using System.Collections;
using UnityEngine.TestTools;
using PullString;
using PullStringTests;

public class ScheduleTimer
{
    [UnityTest]
    public IEnumerator Test_ScheduleTimer()
    {
        yield return new MonoBehaviourTest<ScheduleTimerTest>();
    }

    public class ScheduleTimerTest : TestBase
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
                    TestUtils.TextShouldMatch(response, new[] { "Timer fired" });
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}