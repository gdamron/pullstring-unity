using System.Linq;
using PullString;
using UnityEngine.Assertions;

namespace PullStringTests
{
    public static class TestUtils
    {
        public const string API_KEY = "36890c35-8ecd-4ac4-9538-6c75eb1ea6f6";
        public const string PROJECT = "841cbd2c-e1bf-406b-9efe-a9025399aab4";

        /// <summary>
        /// Compare response with given expected inputs. Test fails if one does not match. If all inputs match and
        /// finished=true, the current integration test will pass.
        /// </summary>
        /// <param name="response">PullString Response object</param>
        /// <param name="expected">An array of expected responses</param>
        public static void TextShouldMatch(Response response, string[] expected)
        {
            int i = 0;
            var outputs = response.Outputs.Where(o => o.Type.Equals(EOutputType.Dialog)).ToArray();

            Assert.IsTrue(expected.Length == outputs.Length, "Number of expected responses is different than acutal responses.");

            foreach (var text in expected)
            {
                Assert.IsTrue(((DialogOutput)outputs[i++]).Text.Equals(text), "Text does not match. Expected " + text + ", found");
            }
        }

        /// <summary>
        /// Compare response with given expected input. Test fails if it does not match. If it does match and
        /// finished=true, the current integration test will pass.
        /// </summary>
        /// <param name="response">PullString Response object</param>
        /// <param name="tests">An Enity representing the expected value</param>
        public static void EntityShouldMatch(Entity[] tests, Response response)
        {
            var entities = response.Entities;

            Assert.IsFalse(entities == null || entities.Length == 0, "Response contains no entities.");
            Assert.IsTrue(entities.Length == tests.Length, "Response contains unexpected number of entities.");

            for (int i = 0; i < tests.Length; i++)
            {
                var expected = tests[i];
                var entity = entities[i];
                var type = expected.Type;

                Assert.IsTrue(expected.Name.Equals(entity.Name), "Entity's name is not the expected one");
                Assert.IsTrue(type.Equals(entity.Type), "Entity is not the expected type");
                Assert.IsFalse((expected.Value != null && entity.Value == null) || (expected.Value == null && entity.Value != null), "Unexpected null value in Entity");
                Assert.IsFalse(type.Equals(EEntityType.Counter) && (double)expected.Value != (double)entity.Value ||
                               type.Equals(EEntityType.Flag) && (bool)expected.Value != (bool)entity.Value ||
                               type.Equals(EEntityType.Label) && !((string)expected.Value).Equals((string)entity.Value),
                               "Entity is expected type but value does not match");

                if (type.Equals(EEntityType.List)) {
                    var testList = (object[])expected.Value;
                    var entList = (object[])entity.Value;

                    Assert.IsTrue(testList.Length == entList.Length, "Expected Entity List length does not match.");

                    for (int j = 0; j < testList.Length; j++) {
                        var jType = testList[j].GetType();

                        Assert.IsTrue(jType == entList[j].GetType(), "Entity List value types do not match");

                        var testVal = testList[j];
                        var entVal = entList[j];

                        Assert.IsFalse(jType == typeof(string) && !((string)testVal).Equals((string)entVal) || 
                                       jType == typeof(double) && (decimal)testVal != (decimal)entVal ||
                                       jType == typeof(bool) && (bool)testVal != (bool)entVal,
                                       "Entity is expected type but value does not match");

                    }
                }
            }
        }

        /// <summary>
        /// Compare response with given expected input. Test fails if it does not match. If it does match and
        /// finished=true, the current integration test will pass.
        /// </summary>
        /// <param name="response">PullString Response object</param>
        /// <param name="expected">A BehaviorOutput object representing the expected value</param>
        public static void BehaviorShouldMatch(BehaviorOutput expected, Response response)
        {
            var behaviors = response.Outputs.Where(o => o.Type.Equals(EOutputType.Behavior)).ToArray();
            Assert.IsTrue(behaviors.Length > 0, "No behaviors found in response.");

            foreach (var b in behaviors)
            {
                var behavior = (BehaviorOutput)b;
                if (behavior.Behavior.Equals(expected.Behavior))
                {
                    if (expected.Parameters != null)
                    {
                        foreach (var kv in expected.Parameters)
                        {
                            Assert.IsTrue(behavior.Parameters.ContainsKey(kv.Key), "Behavior paramters not found.");
                            var param = behavior.Parameters[kv.Key];

                            Assert.IsFalse(!kv.Value.StringValue.Equals(param.StringValue) ||
                                           kv.Value.BoolValue != param.BoolValue ||
                                           (decimal)kv.Value.NumericValue != (decimal)param.NumericValue ||
                                           (kv.Value.ListValue != null && !kv.Value.ListValue.SequenceEqual(param.ListValue)) ||
                                           (kv.Value.DictionaryValue != null && !kv.Value.DictionaryValue.SequenceEqual(param.DictionaryValue)),
                                           "Behavior paramters did not match.");
                        }
                    }
                    // if you made this far, success
                    return;
                }
            }

            Assert.IsTrue(false, "Matching behavior not found.");
        }

        public static void ErrorShouldMatch(string expected, Response response) {
            Assert.IsFalse(response.Status.Success, "Request was successful but should have failed");
            Assert.IsTrue(response.Status.ErrorMessage.StartsWith(expected, System.StringComparison.InvariantCulture), "Received an unexpected error message: " + response.Status.ErrorMessage);
        }

        /// <summary>
        /// Causes test to fail immediately.
        /// </summary>
        public static void ShouldntBeHere()
        {
            Assert.IsTrue(false, "Received and unexpected response");
        }
    }
}