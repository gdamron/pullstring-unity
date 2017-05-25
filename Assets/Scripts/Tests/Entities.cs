using System.Collections;
using UnityEngine.TestTools;
using PullString;
using PullStringTests;

public class Entities
{
    [UnityTest]
    public IEnumerator Test_Entities()
    {
        yield return new MonoBehaviourTest<EntitiesTest>();
    }

    public class EntitiesTest : TestBase
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
                    TestUtils.EntityShouldMatch(new[] { expected }, response);
                    conversation.SetEntities(new[] { label });
                    break;
                case 3:
                    TestUtils.EntityShouldMatch(new[] { label }, response);
                    conversation.SendText("test web service");
                    break;
                case 4:
                    TestUtils.TextShouldMatch(
                        response,
                        new[] { "Web Service Result = INPUT STRING / 42 / 0 / red, green, blue, and purple" }
                    );
                    conversation.GetEntities(new[] { "Number2", "Flag2", "List2" });
                    break;
                case 5:
                    var number2 = new Counter("Number2", 42);
                    var flag2 = new Flag("Flag2", false);
                    var list2 = new List("List2", new[] { "red", "green", "blue", "purple" });
                    TestUtils.EntityShouldMatch(new Entity[] { flag2, list2, number2 }, response);
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}