#if UNIT_TEST

using System.Linq;
using PullString;

namespace PullStringTests
{
    public class TestUtils
    {
        public const string API_KEY = "36890c35-8ecd-4ac4-9538-6c75eb1ea6f6";
        public const string PROJECT = "841cbd2c-e1bf-406b-9efe-a9025399aab4";

        /// <summary>
        /// Compare response with given expected inputs. Test fails if one does not match. If all inputs match and
        /// finished=true, the current integration test will pass.
        /// </summary>
        /// <param name="response">PullString Response object</param>
        /// <param name="expected">An array of expected responses</param>
        /// <param name="finished">[Optional] true if test should pass and exit upon completion</param>
        public static void TextShouldMatch(Response response, string[] expected, bool finished = false)
        {
            int i = 0;
            var outputs = response.Outputs.Where(o => o.Type.Equals(EOutputType.Dialog)).ToArray();

            if (expected.Length != outputs.Length)
            {
                IntegrationTest.Fail("Number of expected responses is different than acutal responses.");
                return;
            }

            foreach (var text in expected)
            {
                if (!((DialogOutput)outputs[i++]).Text.Equals(text))
                {
                    IntegrationTest.Fail("Text does not match. Expected " + text + ", found");
                    return;
                }
            }

            if (finished)
            {
                IntegrationTest.Pass();
            }
        }

        /// <summary>
        /// Compare response with given expected input. Test fails if it does not match. If it does match and
        /// finished=true, the current integration test will pass.
        /// </summary>
        /// <param name="response">PullString Response object</param>
        /// <param name="expected">An Enity representing the expected value</param>
        /// <param name="finished">[Optional] true if test should pass and exit upon completion</param>
        public static void EntityShouldMatch(Entity expected, Response response, bool finished = false)
        {
            var entities = response.Entities;
            var type = expected.Type;
            if (entities == null || entities.Length == 0)
            {
                IntegrationTest.Fail("Response contains no entities.");
                return;
            }
            else if (!expected.Name.Equals(entities[0].Name))
            {
                IntegrationTest.Fail("Entity's name is not the expected one");
                return;
            }
            else if (!expected.Type.Equals(entities[0].Type))
            {
                IntegrationTest.Fail("Entity is not the expected type");
                return;
            }

            if (expected.Value != null && entities[0].Value == null ||
                        expected.Value == null && entities[0].Value != null ||
                        type.Equals(EEntityType.Counter) && (double)expected.Value != (double)entities[0].Value ||
                          type.Equals(EEntityType.Flag) && (bool)expected.Value != (bool)entities[0].Value ||
                        type.Equals(EEntityType.Label) && !((string)expected.Value).Equals((string)entities[0].Value) ||
                        type.Equals(EEntityType.List) && ((object[])expected.Value).SequenceEqual((object[])entities[0].Value))
            {
                IntegrationTest.Fail("Entity is expected type but value does not match");
                return;
            }

            if (finished)
            {
                IntegrationTest.Pass();
            }
        }

        /// <summary>
        /// Compare response with given expected input. Test fails if it does not match. If it does match and
        /// finished=true, the current integration test will pass.
        /// </summary>
        /// <param name="response">PullString Response object</param>
        /// <param name="expected">A BehaviorOutput object representing the expected value</param>
        /// <param name="finished">[Optional] true if test should pass and exit upon completion</param>
        public static void BehaviorShouldMatch(BehaviorOutput expected, Response response, bool finished = false)
        {
            var behaviors = response.Outputs.Where(o => o.Type.Equals(EOutputType.Behavior)).ToArray();
            if (behaviors.Length == 0)
            {
                IntegrationTest.Fail("No behaviors found in response.");
                return;
            }

            foreach (var b in behaviors)
            {
                var behavior = (BehaviorOutput)b;
                if (behavior.Behavior.Equals(expected.Behavior))
                {
                    if (expected.Parameters != null)
                    {
                        foreach (var kv in expected.Parameters)
                        {
                            if (!behavior.Parameters.ContainsKey(kv.Key))
                            {
                                IntegrationTest.Fail("Behavior paramters not found.");
                                return;
                            }

                            var param = behavior.Parameters[kv.Key];

                            if (!kv.Value.StringValue.Equals(param.StringValue) ||
                                kv.Value.BoolValue != param.BoolValue ||
                                kv.Value.NumericValue != param.NumericValue ||
                                (kv.Value.ListValue != null && !kv.Value.ListValue.SequenceEqual(param.ListValue)) ||
                                (kv.Value.DictionaryValue != null && !kv.Value.DictionaryValue.SequenceEqual(param.DictionaryValue)))
                            {
                                IntegrationTest.Fail("Behavior paramters did not match.");
                                return;
                            }
                        }
                    }
                    // if you made it this far, you passed.
                    if (finished)
                    {
                        IntegrationTest.Pass();
                    }
                    return;
                }
            }

            IntegrationTest.Fail("Matching behavior not found.");
        }

        /// <summary>
        /// Causes test to fail immediately.
        /// </summary>
        public static void ShouldntBeHere()
        {
            IntegrationTest.Fail("Received and unexpected response");
        }
    }
}

#endif