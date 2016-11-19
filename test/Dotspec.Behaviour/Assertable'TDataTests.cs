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
    public class Assertable_DataTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new AssertionSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public Assertable_DataTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec and a mocked Assertable fixture object.
            SpecFactory.CreateAssertionSpec<string>(null, null).ReturnsForAnyArgs(fixture.AssertionSpecWithData);
            SpecFactory.CreateAssertable(null, (Action<object, string>)null, null).ReturnsForAnyArgs(fixture.AssertableWithData);

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
            var preconditionData = $"Precondition called {Guid.NewGuid()}";
            var behaviourMessage = "Behaviour called with data: {0}";
            var assertionMessage = "Assertion called with data: {0}";

            scenario.Spec<SubjectSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition, a behaviour and an assertion.
                    precondition = (Func<string>)(() => preconditionData),
                    behaviour = (Action<object, string>)((subject, data) => Events["behaviour"] = string.Format(behaviourMessage, data)),
                    assertion = (Action<object, string>)((subject, data) => Events["assertion"] = string.Format(assertionMessage, data))
                })
                .When(
                    (subject, data) => subject.For(_ => new object())) // When the 'Assert' clause is invoked.
                .Then(
                    // Then validate that precondition, behaviour and assertion 
                    // have been evaluated.
                    (subject, data) => ValidateSpecEvaluation(preconditionData, behaviourMessage, assertionMessage))
                .For(data => new SubjectSpec<object, string>(data.precondition, data.behaviour, data.assertion));
        }

        private void ValidateSpecEvaluation(string preconditionData, string behaviourMessage, string assertionMessage)
        {
            Events["behaviour"].ShouldBe(string.Format(behaviourMessage, preconditionData));
            Events["assertion"].ShouldBe(string.Format(assertionMessage, preconditionData));
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}
