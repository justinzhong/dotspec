using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dotspec.Behaviour
{
    /// <summary>
    /// Validates the behaviour of AssertionSpec class.
    /// </summary>
    public class AssertableTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new AssertionSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public AssertableTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec and a mocked Assertable fixture object.
            SpecFactory.CreateAssertionSpec(null, null).ReturnsForAnyArgs(fixture.AssertionSpec);
            SpecFactory.CreateAssertable(null, null, null).ReturnsForAnyArgs(fixture.Assertable);

            Events = new Dictionary<string, string>();
        }

        /// <summary>
        /// Asserts that the Assertable class have evaluated the test
        /// specification chain:
        /// 1) Precondition
        /// 2) Behaviour
        /// 3) Assertion
        /// </summary>
        [Fact]
        public void SpecShouldBeEvaluated()
        {
            var scenario = "Specification chain should be evaluated";
            var preconditionMessage = "Precondition called";
            var behaviourMessage = "Behaviour called";
            var assertionMessage = "Assertion called";

            scenario.Spec<Assertable<object>>()
                .Given(() => new
                {
                    // Given a precondition, a behaviour and an assertion.
                    precondition = (Action)(() => Events["precondition"] = preconditionMessage),
                    behaviour = (Action<object>)(_ => Events["behaviour"] = behaviourMessage),
                    assertion = (Action<object>)(_ => Events["assertion"] = assertionMessage)
                })
                .When(
                    (subject, data) => subject.Assert(subject)) // When the 'Assert' clause is invoked.
                .Then(
                    // Then validate that precondition, behaviour and assertion 
                    // have been evaluated.
                    (subject, data) => ValidateSpecEvaluation(preconditionMessage, behaviourMessage, assertionMessage))
                .Assert(data => new Assertable<object>(data.precondition, data.behaviour, data.assertion));
        }

        private void ValidateSpecEvaluation(string preconditionMessage, string behaviourMessage, string assertionMessage)
        {
            Events["precondition"].ShouldBe(preconditionMessage);
            Events["behaviour"].ShouldBe(behaviourMessage);
            Events["assertion"].ShouldBe(assertionMessage);
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}
