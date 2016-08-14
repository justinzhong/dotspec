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
                    (sut, data) => sut.When(data.behaviour)) // When the behaviour is passed through to the test subject.
                .Then(
                    // Then validate that the precondition and behaviour were
                    // passed to the SpecFactory.
                    data => ValidateSpecFactoryWithAction(preconditionMessage, behaviourMessage))
                .Assert(data => new BehaviourSpec<object>(data.precondition, SpecFactory));
        }

        /// <summary>
        /// Asserts that the BehaviourSpec will forward the precondition and the
        /// behaviour action to SpecFactory to create an instance of 
        /// IAssertionSpec.
        /// </summary>
        [Fact]
        public void BehaviourWithDataWithWasRegistered()
        {
            var scenario = "Behaviour was registered";
            var preconditionMessage = "Precondition data";

            scenario.Spec<BehaviourSpec<object, string>>()
                .Given(() => new
                {
                    // Given a precondition and a behaviour.
                    precondition = (Func<string>)(() => preconditionMessage),
                    behaviour = (Action<object, string>)((_, data) => Events["behaviour"] = $"Behaviour called with data: {data}")
                })
                .When(
                    (sut, data) => sut.When(data.behaviour)) // When the behaviour is passed through to the test subject.
                .Then(
                    // Then validate that both precondition and behaviour have
                    // been passed to the SpecFactory.
                    _ => ValidateSpecFactoryWithData(preconditionMessage))
                .Assert(data => new BehaviourSpec<object, string>(data.precondition, SpecFactory));
        }

        private bool ValidateBehaviourWithData(Action<object, string> behaviour, string preconditionMessage)
        {
            behaviour(new object(), preconditionMessage);
            Events.Count.ShouldBe(1);
            Events["behaviour"].ShouldBe($"Behaviour called with data: {preconditionMessage}");

            return true;
        }

        private bool ValidatePreconditionWithData(Func<string> precondition, string preconditionMessage)
        {
            var actualMessage = precondition();
            actualMessage.ShouldBe(preconditionMessage);

            return true;
        }

        private void ValidateSpecFactoryWithData(string dataMessage)
        {
            // Validate that both precondition and behaviour have been passed to
            // the SpecFactory.
            SpecFactory.Received(1).CreateAssertionSpec(
                Arg.Is<Func<string>>(arg => ValidatePreconditionWithData(arg, dataMessage)),
                Arg.Is<Action<object, string>>(arg => ValidateBehaviourWithData(arg, dataMessage)));
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