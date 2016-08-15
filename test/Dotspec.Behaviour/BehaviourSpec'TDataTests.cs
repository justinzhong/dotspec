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
    public class BehaviourSpec_DataTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        private Dictionary<string, string> Events { get; }

        /// <summary>
        /// Instantiates a new BehaviourSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public BehaviourSpec_DataTests(SpecFactoryFixture fixture)
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
            var preconditionData = $"Precondition data {Guid.NewGuid()}";
            var behaviourMessage = "Behaviour called with data: {0}";

            scenario.Spec<BehaviourSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition and a behaviour.
                    precondition = (Func<string>)(() => preconditionData),
                    behaviour = (Action<object, string>)((_, data) => Events["behaviour"] = string.Format(behaviourMessage, preconditionData))
                })
                .When(
                    (subject, data) => subject.When(data.behaviour)) // When the behaviour is passed through to the test subject.
                .Then(
                    // Then validate that both precondition and behaviour have
                    // been passed to the SpecFactory.
                    (subject, data) => ValidateSpecFactory(behaviourMessage, preconditionData))
                .Assert(data => new BehaviourSpec<object, string>(data.precondition, SpecFactory));
        }

        private bool ValidateBehaviour(Action<object, string> behaviour, string behaviourMessage, string preconditionData)
        {
            behaviour(new object(), preconditionData);
            Events["behaviour"].ShouldBe(string.Format(behaviourMessage, preconditionData));

            return true;
        }

        private bool ValidatePrecondition(Func<string> precondition, string preconditionMessage)
        {
            precondition().ShouldBe(preconditionMessage);

            return true;
        }

        private void ValidateSpecFactory(string behaviourMessage, string preconditionData)
        {
            // Validate that both precondition and behaviour have been passed to
            // the SpecFactory.
            SpecFactory.Received(1).CreateAssertionSpec(
                Arg.Is<Func<string>>(arg => ValidatePrecondition(arg, preconditionData)),
                Arg.Is<Action<object, string>>(arg => ValidateBehaviour(arg, behaviourMessage, preconditionData)));
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}