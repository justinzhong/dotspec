using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dotspec.Behaviour
{
    public class ResultAssertableTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new ResultAssertableTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public ResultAssertableTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec and a mocked Assertable fixture object.
            SpecFactory.CreateAssertionSpec(null, null).ReturnsForAnyArgs(fixture.AssertionSpec);

            Events = new Dictionary<string, string>();
        }

        /// <summary>
        /// Asserts that the ResultAssertable class evaluated the test 
        /// specification steps in the following order:
        /// 1) Precondition
        /// 2) Behaviour
        /// 3) Assertion
        /// </summary>
        [Fact]
        public void AllSpecStepsShouldBeEvaluated()
        {
            var preconditionMessage = "Precondition called";
            var behaviourResult = Guid.NewGuid();
            var assertionMessage = "Assertion called with result: {0}";
            var expectedAssertionMessage = string.Format(assertionMessage, behaviourResult);
            var sequence = 0;

            "All test specification steps should have been evaluated".Spec<SubjectResultSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition, a behaviour and an assertion.
                    precondition = BuildPrecondition(++sequence, preconditionMessage),
                    behaviour = BuildBehaviour(++sequence, behaviourResult.ToString()),
                    assertion = BuildAssertion(++sequence, assertionMessage)
                })
                .When(
                    (subject, data) => subject.For(subject)) // When the 'Assert' clause is invoked.
                .Then(
                    // Then validate that precondition, behaviour and assertion 
                    // have been passed to the SpecFactory.
                    (subject, data) => ValidateOutcome(preconditionMessage, behaviourResult, expectedAssertionMessage))
                .For(data => new SubjectResultSpec<object, string>(data.precondition, data.behaviour, data.assertion));
        }

        private Action BuildPrecondition(int sequence, string preconditionMessage)
        {
            return () => Events[$"{sequence}.precondition"] = preconditionMessage;
        }

        private Func<object, string> BuildBehaviour(int sequence, string behaviourResult)
        {
            return _ =>
            {
                Events[$"{sequence}.behaviour"] = behaviourResult;

                return behaviourResult;
            };
        }

        private Action<object, string> BuildAssertion(int sequence, string assertionMessage)
        {
            return (_, behaviourResult) => Events[$"{sequence}.assertion"] = string.Format(assertionMessage, behaviourResult);
        }

        private void ValidateOutcome(string preconditionMessage, Guid behaviourResult, string expectedAssertionMessage)
        {
            Events["1.precondition"].Equals(preconditionMessage);
            Events["2.behaviour"].Equals(behaviourResult.ToString());
            Events["3.assertion"].Equals(expectedAssertionMessage);
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}
