using NSubstitute;

namespace Dotspec.Behaviour
{
    public class SpecFactoryFixture
    {
        public ISpecFactory<object> SpecFactory { get; }
        public IBehaviourSpec<object> BehaviourSpec { get; }

        public SpecFactoryFixture()
        {
            SpecFactory = Substitute.For<ISpecFactory<object>>();
            BehaviourSpec = Substitute.For<IBehaviourSpec<object>>();

            SpecFactory.CreateBehaviourSpec(null).ReturnsForAnyArgs(BehaviourSpec);
        }
    }
}