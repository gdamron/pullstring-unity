#if UNIT_TEST

using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("Go to response")]
public class GoToResponse : TestBase
{
    override protected void Run(Response response, int step)
    {
        switch (step)
        {
            case 0:
                var guid = "d6701507-61a9-47d9-8300-2e9c6b08dfcd";
                conversation.GoTo(guid);
                break;
            case 1:
                TestUtils.TextShouldMatch(response, new[] { "Hello " }, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif