using System;
using NSubstitute;
using Xunit;

namespace Dotspec.Behaviour
{
    /// <summary>
    /// Validates the behaviour of PreconditionSpec class.
    /// </summary>
    public class PreconditionSpecTests : IClassFixture<SpecFactoryFixture>
    {
        private ISpecFactory<object> SpecFactory { get; }

        /// <summary>
        /// Instantiates a new PreconditionSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public PreconditionSpecTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked BehaviourSpec
            SpecFactory.CreateBehaviourSpec(null).ReturnsForAnyArgs(fixture.BehaviourSpec);
        }

        /// <summary>
        /// Asserts that when given a precondition the PreconditionSpec will
        /// pass that to SpecFactory to instantiate an instance of 
        /// IBehaviourSpec.
        /// 
        /// The precondition is declared as a variable outside the scope of
        /// the PreconditionSpec instance.
        /// </summary>
        [Fact]
        public void PreconditionWasRegistered()
        {
            var scenario = "Precondition was registered";
            Action expectedPrecondition = null;

            scenario.Spec<PreconditionSpec<object>>()
                .Given(
                    expectedPrecondition = () => { }) // Setup the precondition that's been defined as variable
                .When(
                    (subject) => subject.Given(expectedPrecondition))
                .Then(
                    () => SpecFactory.CreateBehaviourSpec(Arg.Is(expectedPrecondition)).Received(1))
                .Assert(new PreconditionSpec<object>(scenario, SpecFactory));
        }

        /// <summary>
        /// Asserts that when given a precondition the PreconditionSpec will
        /// pass that to SpecFactory to instantiate an instance of 
        /// IBehaviourSpec.
        /// 
        /// The precondition is declared as an inline data within the scope of 
        /// the PreconditionSpec instance.
        /// </summary>
        [Fact]
        public void PreconditionWithDataWasRegistered()
        {
            var scenario = "Precondition was registered";

            scenario.Spec<PreconditionSpec<object>>()
                .Given(
                    () => (Action)(() => { })) // Setup the precondition and return as inline data
                .When(
                    (subject, expectedPrecondition) => subject.Given(expectedPrecondition))
                .Then(
                    expectedPrecondition => SpecFactory.CreateBehaviourSpec(Arg.Is(expectedPrecondition)).Received(1))
                .Assert(new PreconditionSpec<object>(scenario, SpecFactory));
        }
    }
}