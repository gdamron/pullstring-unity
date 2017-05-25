using System.Collections;
using UnityEngine.TestTools;
using PullString;
using PullStringTests;

public class IntroIntent
{
	[UnityTest]
	public IEnumerator Test_IntroIntent()
	{
		yield return new MonoBehaviourTest<IntroIntentTest>();
	}

    public class IntroIntentTest : TestBase
    {
        public override string Project
        {
            get
            {
                return "176a87fb-4d3c-fde5-4b3c-54f18c2d99a4";
            }
        }

        protected override void Run(Response response, int step)
        {
            switch (step)
            {
                case 0:
                    TestUtils.TextShouldMatch(response, new[] { "Welcome to the LUIS test. What do you like?" });
                    Label colorLabel = new Label("Luis Color", "Green");
                    conversation.SendIntent("Favorite Color", new[] { colorLabel });
                    break;
                case 1:
                    TestUtils.TextShouldMatch(response, new[] { "Green is a cool color", "Tell me another color" });
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}