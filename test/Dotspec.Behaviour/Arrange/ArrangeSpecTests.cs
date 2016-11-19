using System;
using Dotspec.Arrange;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Dotspec.Behaviour.Arrange
{
    /// <summary>
    /// Validates the behaviour of PreconditionSpec class.
    /// </summary>
    public class ArrangeSpecTests : IClassFixture<SpecFactoryFixture>, IDisposable
    {
        private ISpecFactory<object> SpecFactory { get; }

        /// <summary>
        /// Instantiates a new PreconditionSpecTests object.
        /// </summary>
        /// <param name="fixture"></param>
        public ArrangeSpecTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;

            // Returns a mocked BehaviourSpec
            SpecFactory.CreateBehaviourSpec(null).ReturnsForAnyArgs(fixture.BehaviourSpec);
        }

        /// <summary>
        /// When given a precondition the ArrangeSpec instance will
        /// pass that to SpecFactory to instantiate an instance of 
        /// IArrangeSpec.
        /// 
        /// The precondition is declared as a variable outside the scope of
        /// the ArrangeSpec instance.
        /// </summary>
        [Fact]
        public void PreconditionWasRegistered()
        {
            var scenario = "Precondition (function) was registered";
            var seed = Guid.NewGuid();

            scenario.Spec<ArrangeSpec<object>>()
                .Given(
                    () => (Func<object>)(() => seed)) // Setup the precondition that's been defined as variable
                .When(
                    (arrangeSpec, precondition) => arrangeSpec.Given(precondition))
                .Then(
                    (precondition) =>
                    {
                        SpecFactory
                            .Received(1)
                            .CreateBehaviourSpec(Arg.Is(precondition));

                        precondition().ShouldBe(seed);
                    })
                .For(new ArrangeSpec<object>(scenario, SpecFactory));
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
            var scenario = "Precondition (action) was registered";

            scenario.Spec<ArrangeSpec<object>>()
                .Given(
                    () => (Action)(() => { })) // Setup the precondition and return as inline data
                .When(
                    (subject, precondition) => subject.Given(precondition))
                .Then(
                    (precondition) =>
                    {
                        SpecFactory
                            .Received(1)
                            .CreateBehaviourSpec(Arg.Is(precondition));
                    })
                .For(new ArrangeSpec<object>(scenario, SpecFactory));
        }

        public void Dispose()
        {
            SpecFactory.ClearReceivedCalls();
        }
    }
}