using System;
using NSubstitute;
using Xunit;

namespace Dotspec.Behaviour
{
    public class PreconditionSpecTests : IClassFixture<SpecFactoryFixture>
    {
        private ISpecFactory<object> SpecFactory { get; }

        public PreconditionSpecTests(SpecFactoryFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            SpecFactory = fixture.SpecFactory;
        }

        [Fact]
        public void PreconditionWasRegistered()
        {
            var scenario = "Precondition was registered";
            Action expectedPrecondition = () => { };

            scenario.Spec<PreconditionSpec<object>>()
                .Given(
                    expectedPrecondition) // Expected precondition
                .When(
                    (subject) => subject.Given(expectedPrecondition))
                .Then(
                    () => SpecFactory.CreateBehaviourSpec(Arg.Is(expectedPrecondition)).Received(1))
                .Assert(new PreconditionSpec<object>(scenario, SpecFactory));
        }

        [Fact]
        public void PreconditionWithDataWasRegistered()
        {
            var scenario = "Precondition was registered";

            scenario.Spec<PreconditionSpec<object>>()
                .Given(
                    () => (Action)(() => { })) // Expected precondition
                .When(
                    (subject, expectedPrecondition) => subject.Given(expectedPrecondition))
                .Then(
                    expectedPrecondition => SpecFactory.CreateBehaviourSpec(Arg.Is(expectedPrecondition)).Received(1))
                .Assert(new PreconditionSpec<object>(scenario, SpecFactory));
        }
    }
}