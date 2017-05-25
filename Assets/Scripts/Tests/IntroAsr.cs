using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using System;
using PullString;
using PullStringTests;

public class IntroAsr
{
    [UnityTest]
    public IEnumerator Test_IntroAsr()
    {
        yield return new MonoBehaviourTest<IntroAsrTest>();
    }

    public class IntroAsrTest : TestBase
    {
        AudioSource source;
        protected override void Run(Response response, int step)
        {
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.clip = Resources.Load<AudioClip>("asrTest");
            }

            switch (step)
            {
                case 0:
                    TestUtils.TextShouldMatch(response, new[] { "Hello. What's your name?" });
                    try
                    {
                        conversation.SendAudio(source.clip);
                    }
                    catch (Exception e)
                    {
                        Assert.IsTrue(false, "Unable to load audio: " + e);
                        return;
                    }
                    break;
                case 1:
                    TestUtils.TextShouldMatch(response, new[] { "Hello Grant" });
                    IsTestFinished = true;
                    break;
                default:
                    TestUtils.ShouldntBeHere();
                    break;
            }
        }
    }
}