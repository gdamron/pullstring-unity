#if UNIT_TEST

using UnityEngine;
using PullString;

namespace PullStringTests
{
    public class TestBase : MonoBehaviour
    {
        protected Conversation conversation;
        protected Request request = null;
        protected int step = 0;

        public void Awake()
        {
            conversation = gameObject.AddComponent<Conversation>();
            request = new Request()
            {
                ApiKey = TestUtils.API_KEY
            };

            conversation.OnResponseReceived += OnResponse;
        }

        public void Start()
        {
            conversation.Begin(TestUtils.PROJECT, request);
        }

        void OnResponse(Response response)
        {
            Run(response, step);
            step++;
        }

        /// <summary>
        /// Override in subclass to run a series of inputs and responses.
        /// <example>
        /// <code>
        /// switch (step)
        /// {
        ///    case 0:
        ///        TestUtils.TextShouldMatch(response, new[] { "Hello. What's your name?" });
        ///        conversation.SendText("janet");
        ///        break;
        ///    case 1:
        ///        TestUtils.TextShouldMatch(response, new[] { "Hello Janet" });
        ///        var state = conversation.ParticipantId;
        ///        request.ParticipantId = state;
        ///        request.ConversationId = null;
        ///        conversation.Begin(TestUtils.PROJECT, request);
        ///        break;
        ///    case 2:
        ///        TestUtils.TextShouldMatch(response, new[] { "Welcome back JANET" }, true);
        ///        break;
        ///    default:
        ///        TestUtils.ShouldntBeHere();
        ///        break;
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="response">The PullString Web API Response</param>
        /// <param name="step">Current iteration of the method</param>
        protected virtual void Run(Response response, int step) { }
    }
}

#endif