using System;
using NSubstitute;
using Xunit;
using Shouldly;

namespace Dotspec.Behaviour
{
    /// <summary>
    /// Validates the behaviour of PreconditionSpec class.
    /// </summary>
    public class PreconditionSpecTests : IClassFixture<SpecFactoryFixture>, IDisposable
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
                    () => 
                    {
                        SpecFactory
                            .Received(1)
                            .CreateBehaviourSpec(Arg.Do<Action>(arg => arg.ShouldBe(expectedPrecondition)));
                    })
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
                    expectedPrecondition => 
                    {
                        SpecFactory
                            .Received(1)
                            .CreateBehaviourSpec(Arg.Do<Action>(arg => arg.ShouldBe(expectedPrecondition)));
                    })
                .Assert(new PreconditionSpec<object>(scenario, SpecFactory));
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
        }
    }
}