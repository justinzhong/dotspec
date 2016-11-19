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
    public class AssertionSpec_DataTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new AssertionSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public AssertionSpec_DataTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec and a mocked Assertable fixture object.
            SpecFactory.CreateAssertionSpec<string>(null, null).ReturnsForAnyArgs(fixture.AssertionSpecWithData);
            SpecFactory.CreateAssertable(null, (Action<object, string>)null, null).ReturnsForAnyArgs(fixture.AssertableWithData);

            Events = new Dictionary<string, string>();
        }

        /// <summary>
        /// Asserts that the Assertion class passed the following to the 
        /// SpecFactory to create an instance of IAssertionSpec.
        /// 1) Precondition
        /// 2) Behaviour
        /// 3) Assertion
        /// </summary>
        [Fact]
        public void AssertionWasRegistered()
        {
            var scenario = "Assertion was registered";
            var preconditionData = $"Precondition data {Guid.NewGuid()}";
            var behaviourMessage = "Behaviour called with data: {0}";
            var assertionMessage = "Assertion called with data: {0}";

            scenario.Spec<AssertionSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition, a behaviour and an assertion.
                    precondition = (Func<string>)(() => preconditionData),
                    behaviour = (Action<object, string>)((subject, data) => Events["behaviour"] = string.Format(behaviourMessage, preconditionData)),
                    assertion = (Action<object, string>)((subject, data) => Events["assertion"] = string.Format(assertionMessage, preconditionData))
                })
                .When(
                    (subject, data) => subject.Then(data.assertion)) // When the 'Then' clause is invoked.
                .Then(
                    // Then validate that precondition, behaviour and assertion 
                    // have been passed to the SpecFactory.
                    (subject, data) => ValidateSpecFactory(preconditionData, behaviourMessage, assertionMessage))
                .For(data => new AssertionSpec<object, string>(data.precondition, data.behaviour, SpecFactory));
        }

        private void ValidateSpecFactory(string preconditionData, string behaviourMessage, string assertionMessage)
        {
            SpecFactory.Received(1).CreateAssertable<string>(
                Arg.Is<Func<string>>(arg => ValidatePrecondition(arg, preconditionData)),
                Arg.Is<Action<object, string>>(arg => ValidateBehaviour(arg, behaviourMessage, preconditionData)),
                Arg.Is<Action<object, string>>(arg => ValidateAssertion(arg, assertionMessage, preconditionData)));
        }

        private bool ValidateAssertion(Action<object, string> assertion, string assertionMessage, string preconditionData)
        {
            assertion(new object(), preconditionData);
            Events["assertion"].ShouldBe(string.Format(assertionMessage, preconditionData));

            return true;
        }

        private bool ValidateBehaviour(Action<object, string> behaviour, string behaviourMessage, string preconditionData)
        {
            behaviour(new object(), preconditionData);
            Events["behaviour"].ShouldBe(string.Format(behaviourMessage, preconditionData));

            return true;
        }

        private bool ValidatePrecondition(Func<string> precondition, string preconditionData)
        {
            precondition().ShouldBe(preconditionData);

            return true;
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}
