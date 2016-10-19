#if UNIT_TEST

using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("Entities")]
public class Entities : TestBase
{
    Label label = new Label("NAME", "jill");

    protected override void Run(Response response, int step)
    {
        switch (step)
        {
            case 0:
                TestUtils.TextShouldMatch(response, new[] { "Hello. What's your name?" });
                conversation.SendText("jack");
                break;
            case 1:
                TestUtils.TextShouldMatch(response, new[] { "Hello Jack" });
                conversation.GetEntities(new[] { "NAME" });
                break;
            case 2:
                var expected = new Label("NAME", "jack");
                TestUtils.EntityShouldMatch(expected, response);
                conversation.SetEntities(new[] { label });
                break;
            case 3:
                TestUtils.EntityShouldMatch(label, response, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif