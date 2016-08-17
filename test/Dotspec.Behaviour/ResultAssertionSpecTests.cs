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
    public class ResultAssertionSpecTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new ResultAssertionSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public ResultAssertionSpecTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec and a mocked Assertable fixture object.
            SpecFactory.CreateAssertionSpec(null, null).ReturnsForAnyArgs(fixture.AssertionSpec);
            SpecFactory.CreateAssertable(null, null, null).ReturnsForAnyArgs(fixture.Assertable);

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
            var preconditionMessage = "Precondition called";
            var behaviourMessage = $"Behaviour result {Guid.NewGuid()}";
            var assertionMessage = "Assertion called";

            scenario.Spec<ResultAssertionSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition, a behaviour and an assertion.
                    precondition = (Action)(() => Events["precondition"] = preconditionMessage),
                    behaviour = (Func<object, string>)(_ => behaviourMessage),
                    assertion = (Action<object>)(_ => Events["assertion"] = assertionMessage)
                })
                .When(
                    (subject, data) => subject.Then(data.assertion)) // When the 'Then' clause is invoked.
                .Then(
                    // Then validate that precondition, behaviour and assertion 
                    // have been passed to the SpecFactory.
                    (subject, data) => ValidateSpecFactory(preconditionMessage, behaviourMessage, assertionMessage))
                .Assert(data => new ResultAssertionSpec<object, string>(data.precondition, data.behaviour, SpecFactory));
        }

        /// <summary>
        /// Asserts that the Assertion class passed the following to the 
        /// SpecFactory to create an instance of IAssertionSpec.
        /// 1) Precondition
        /// 2) Behaviour
        /// 3) Assertion
        /// </summary>
        [Fact]
        public void AssertionWithResultWasRegistered()
        {
            var scenario = "Assertion with result was registered";
            var preconditionMessage = "Precondition called";
            var behaviourMessage = $"Behaviour result {Guid.NewGuid()}";
            var assertionMessage = "Assertion called with result {0}";
            var expectedAssertionMessage = string.Format(assertionMessage, behaviourMessage);

            scenario.Spec<ResultAssertionSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition, a behaviour and an assertion.
                    precondition = (Action)(() => Events["precondition"] = preconditionMessage),
                    behaviour = (Func<object, string>)(_ => behaviourMessage),
                    assertion = (Action<object, string>)((_, data) => Events["assertion"] = string.Format(assertionMessage, data))
                })
                .When(
                    (subject, data) => subject.Then(data.assertion)) // When the 'Then' clause is invoked.
                .Then(
                    // Then validate that precondition, behaviour and assertion 
                    // have been passed to the SpecFactory.
                    (subject, data) => ValidateSpecFactoryWithResult(preconditionMessage, behaviourMessage, expectedAssertionMessage))
                .Assert(data => new ResultAssertionSpec<object, string>(data.precondition, data.behaviour, SpecFactory));
        }

        private void ValidateSpecFactoryWithResult(string preconditionMessage, string behaviourMessage, string assertionMessage)
        {
            SpecFactory.Received(1).CreateAssertable(
                Arg.Is<Action>(arg => ValidatePrecondition(arg, preconditionMessage)),
                Arg.Is<Func<object, string>>(arg => ValidateBehaviour(arg, behaviourMessage)),
                Arg.Is<Action<object, string>>(arg => ValidateAssertionWithResult(arg, behaviourMessage, assertionMessage)));
        }

        private void ValidateSpecFactory(string preconditionMessage, string behaviourMessage, string assertionMessage)
        {
            SpecFactory.Received(1).CreateAssertable(
                Arg.Is<Action>(arg => ValidatePrecondition(arg, preconditionMessage)),
                Arg.Is<Func<object, string>>(arg => ValidateBehaviour(arg, behaviourMessage)),
                Arg.Is<Action<object, string>>(arg => ValidateAssertion(arg, assertionMessage)));
        }

        private bool ValidateAssertion(Action<object, string> assertion, string assertionMessage)
        {
            assertion(new object(), null);
            Events["assertion"].ShouldBe(assertionMessage);

            return true;
        }

        private bool ValidateAssertionWithResult(Action<object, string> assertion, string behaviourMessage, string assertionMessage)
        {
            assertion(new object(), behaviourMessage);
            Events["assertion"].ShouldBe(assertionMessage);

            return true;
        }

        private bool ValidateBehaviour(Func<object, string> behaviour, string behaviourMessage)
        {
            behaviour(new object()).ShouldBe(behaviourMessage);

            return true;
        }

        private bool ValidatePrecondition(Action precondition, string preconditionMessage)
        {
            precondition();
            Events["precondition"].ShouldBe(preconditionMessage);

            return true;
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}
