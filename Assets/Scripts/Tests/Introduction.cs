#if UNIT_TEST

using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("Introduction")]
public class Introduction : TestBase
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
                TestUtils.TextShouldMatch(response, new[] { "Welcome back JANET" }, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif