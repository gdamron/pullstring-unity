using System.Collections;
using UnityEngine.TestTools;
using PullString;
using PullStringTests;

public class BadProject
{
    [UnityTest]
    public IEnumerator Test_BadProject()
    {
        yield return new MonoBehaviourTest<BadProjectTest>();
    }

    public class BadProjectTest : TestBase
    {
        public override string Project
        {
            get
            {
                return "crapcrapcrap";
            }
        }

        protected override void Run(Response response, int step)
        {
            var apiKey = Project;
            switch (step)
            {
                case 0:
                    TestUtils.ErrorShouldMatch("Invalid project for API key", response);
                    request.ApiKey = apiKey;
                    conversation.Begin(TestUtils.PROJECT, request);
                    break;
                case 1:
                    TestUtils.ErrorShouldMatch("Invalid Authorization Bearer Token", response);
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}