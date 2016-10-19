#if UNIT_TEST

using UnityEngine;
using System;
using PullString;
using PullStringTests;

[IntegrationTest.DynamicTest("ASR Introduction")]
public class IntroAsr : TestBase
{
    AudioSource source = null;
    protected override void Run(Response response, int step)
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
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
                    IntegrationTest.Fail("Unable to loaad audio: " + e);
                    return;
                }
                break;
            case 1:
                TestUtils.TextShouldMatch(response, new[] { "Hello Grant" }, true);
                break;
            default:
                TestUtils.ShouldntBeHere();
                break;
        }
    }
}

#endif