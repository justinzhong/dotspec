using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dotspec.Behaviour
{
    /// <summary>
    /// Validates the behaviour of BehaviourSpecTests class.
    /// </summary>
    public class BehaviourSpecTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new BehaviourSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public BehaviourSpecTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec
            SpecFactory.CreateAssertionSpec(null, null).ReturnsForAnyArgs(fixture.AssertionSpec);

            Events = new Dictionary<string, string>();
        }

        /// <summary>
        /// Asserts that the BehaviourSpec will forward the precondition and the
        /// behaviour action to SpecFactory to create an instance of 
        /// IAssertionSpec.
        /// </summary>
        [Fact]
        public void BehaviourWasRegistered()
        {
            var scenario = "Behaviour was registered";
            var preconditionMessage = "Precondition called";
            var behaviourMessage = "Behaviour called";

            scenario.Spec<BehaviourSpec<object>>()
                .Given(() => new
                {
                    // Given a precondition and a behaviour.
                    precondition = (Action)(() => Events["precondition"] = preconditionMessage),
                    behaviour = (Action<object>)(_ => Events["behaviour"] = behaviourMessage)
                })
                .When(
                    (subject, data) => subject.When(data.behaviour)) // When the 'When' clause is invoked.
                .Then(
                    // Then validate that the precondition and behaviour were
                    // passed to the SpecFactory.
                    (subject, data) => ValidateSpecFactoryWithAction(preconditionMessage, behaviourMessage))
                .Assert(data => new BehaviourSpec<object>(data.precondition, SpecFactory));
        }

        private bool ValidatePreconditionWithMessage(Action precondition, string preconditionMessage)
        {
            precondition();
            Events["precondition"].ShouldBe(preconditionMessage);

            return true;
        }

        private bool ValidateBehaviourWithMessage(Action<object> behaviour, string behaviourMessage)
        {
            behaviour(new object());
            Events["behaviour"].ShouldBe(behaviourMessage);

            return true;
        }

        private bool ValidateSpecFactoryWithAction(string preconditionMessage, string behaviourMessage)
        {
            SpecFactory.Received(1).CreateAssertionSpec(
                Arg.Is<Action>(arg => ValidatePreconditionWithMessage(arg, preconditionMessage)), 
                Arg.Is<Action<object>>(arg => ValidateBehaviourWithMessage(arg, behaviourMessage)));

            return true;
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}