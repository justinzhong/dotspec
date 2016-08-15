using NSubstitute;

namespace Dotspec.Behaviour
{
    public class SpecFactoryFixture
    {
        public ISpecFactory<object> SpecFactory { get; }

        public IBehaviourSpec<object> BehaviourSpec { get; }

        public IAssertionSpec<object> AssertionSpec { get; }

        public IAssertionSpec<object, string> AssertionSpecWithData { get; }

        public IAssertable<object> Assertable { get; }

        public IAssertable<object, string> AssertableWithData { get; }

        public SpecFactoryFixture()
        {
            SpecFactory = Substitute.For<ISpecFactory<object>>();
            BehaviourSpec = Substitute.For<IBehaviourSpec<object>>();
            AssertionSpec = Substitute.For<IAssertionSpec<object>>();
            AssertionSpecWithData = Substitute.For<IAssertionSpec<object, string>>();
            Assertable = Substitute.For<IAssertable<object>>();
            AssertableWithData = Substitute.For<IAssertable<object, string>>();
        }
    }
}