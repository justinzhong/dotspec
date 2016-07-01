using NSubstitute;

namespace Dotspec.Behaviour
{
    public class SpecFactoryFixture
    {
        public ISpecFactory<object> SpecFactory { get; }

        public IBehaviourSpec<object> BehaviourSpec { get; }

        public IAssertionSpec<object> AssertionSpec { get; }

        public SpecFactoryFixture()
        {
            SpecFactory = Substitute.For<ISpecFactory<object>>();
            BehaviourSpec = Substitute.For<IBehaviourSpec<object>>();
            AssertionSpec = Substitute.For<IAssertionSpec<object>>();
        }
    }
}