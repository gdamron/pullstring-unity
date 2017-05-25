using System.Collections;
using UnityEngine.TestTools;
using PullString;
using PullStringTests;

public class GoToResponse
{
    [UnityTest]
    public IEnumerator Test_GoToResponse()
    {
        yield return new MonoBehaviourTest<GoToResponseTest>();
    }

    public class GoToResponseTest : TestBase
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
                    TestUtils.TextShouldMatch(response, new[] { "Hello " });
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}