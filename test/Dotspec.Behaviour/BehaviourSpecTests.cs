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

        private List<string> Events { get; }

        /// <summary>
        /// Instantiates a new BehaviourSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public BehaviourSpecTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked AssertionSpec
            SpecFactory.CreateAssertionSpec(null).ReturnsForAnyArgs(fixture.AssertionSpec);

            Events = new List<string>();
        }

        /// <summary>
        /// Asserts that the BehaviourSpec will bundle a pipeline of 
        /// precondition followed by the behaviour action before passing over to
        /// SpecFactory to instantiate an instance of IAssertionSpec.
        /// </summary>
        [Fact]
        public void BehaviourWasRegistered()
        {
            var scenario = "Behaviour was registered";

            scenario.Spec<BehaviourSpec<object>>()
                .Given(() => new
                {
                    precondition = (Action)(() => Events.Add("Precondition called")),
                    behaviour = (Action<object>)(_ => Events.Add("Behaviour called"))
                }) // Given a precondition and a behaviour lambda expression.
                .When(
                    (sut, data) => sut.When(data.behaviour)) // When the behaviour is passed through to the test subject.
                .Then(
                    // Then a pipeline of precondition followed by behaviour should be passed to
                    // SpecFactory to create the AssertionSpec object.
                    _ => SpecFactory.Received(1).CreateAssertionSpec(Arg.Is<Action<object>>(pipeline => ValidateArg(pipeline))))
                .Assert(data => new BehaviourSpec<object>(data.precondition, SpecFactory));
        }

        private bool ValidateArg(Action<object> pipeline)
        {
            // The argument is expected to contain both the precondition and
            // behaviour actions.
            //
            // After evaluating the argument, there should be two events; 
            // precondition and behaviour, in that order.
            pipeline(new object());

            Events.Count.ShouldBe(2);
            Events[0].ShouldBe("Precondition called");
            Events[1].ShouldBe("Behaviour called");

            return true;
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
            Events.Clear();
        }
    }
}