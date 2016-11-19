using NSubstitute;

namespace Dotspec.Behaviour
{
    public class SpecFactoryFixture
    {
        public ISpecFactory<object> SpecFactory { get; }

        public IActSpec<object> BehaviourSpec { get; }

        public IAssertionSpec<object> AssertionSpec { get; }

        public IAssertionSpec<object, string> AssertionSpecWithData { get; }

        public ISubjectSpec<object> Assertable { get; }

        public ISubjectSpec<object, string> AssertableWithData { get; }

        public SpecFactoryFixture()
        {
            SpecFactory = Substitute.For<ISpecFactory<object>>();
            BehaviourSpec = Substitute.For<IActSpec<object>>();
            AssertionSpec = Substitute.For<IAssertionSpec<object>>();
            AssertionSpecWithData = Substitute.For<IAssertionSpec<object, string>>();
            Assertable = Substitute.For<ISubjectSpec<object>>();
            AssertableWithData = Substitute.For<ISubjectSpec<object, string>>();
        }
    }
}