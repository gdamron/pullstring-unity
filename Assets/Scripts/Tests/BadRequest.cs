using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using PullString;

public class BadRequest
{
    [UnityTest]
    public IEnumerator Test_BadRequest()
    {
        yield return new MonoBehaviourTest<BadRequestTest>();
    }

    public class BadRequestTest : MonoBehaviour, IMonoBehaviourTest
    {
        public bool IsTestFinished { get; set; }

        void Start()
        {
            var conversation = gameObject.AddComponent<Conversation>();

            Assert.IsTrue(string.IsNullOrEmpty(conversation.ConversationId), "Conversation Id should be empty.");
            Assert.IsTrue(string.IsNullOrEmpty(conversation.ParticipantId), "Participant Id should be empty.");

            conversation.OnResponseReceived += OnResponse;
            try
            {
                conversation.Begin(null, null);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Conversation: valid Request object missing."))
                {
                    IsTestFinished = true;
                }
                else
                {
                    Assert.IsTrue(false, "Received unexpected error: " + e.Message);
                }
            }
        }

        void OnResponse(Response response)
        {
            Assert.IsTrue(false, "Conversation should not have started with null parameters");
        }
    }
}