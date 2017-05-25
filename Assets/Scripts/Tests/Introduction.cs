using System.Collections;
using UnityEngine.TestTools;
using PullString;
using PullStringTests;

public class Introduction
{
    [UnityTest]
    public IEnumerator Test_Introduction()
    {
        yield return new MonoBehaviourTest<IntroductionTest>();
    }

    public class IntroductionTest : TestBase
    {
        protected override void Run(Response response, int step)
        {
            switch (step)
            {
                case 0:
                    TestUtils.TextShouldMatch(response, new[] { "Hello. What's your name?" });
                    conversation.SendText("janet");
                    break;
                case 1:
                    TestUtils.TextShouldMatch(response, new[] { "Hello Janet" });
                    var state = conversation.ParticipantId;
                    request.ParticipantId = state;
                    request.ConversationId = null;
                    conversation.Begin(TestUtils.PROJECT, request);
                    break;
                case 2:
                    TestUtils.TextShouldMatch(response, new[] { "Welcome back JANET" });
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}