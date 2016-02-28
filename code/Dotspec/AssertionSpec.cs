using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject, TData, TResult> : SpecBase<TSubject>
        where TSubject : class
    {
        private readonly TData _data;
        private readonly Func<TResult> _resultFunc;
        private Action<TData, TResult> _assertion;

        public AssertionSpec(string scenario, TData data, Func<TResult> resultFunc) : base(scenario)
        {
            _data = data;
            _resultFunc = resultFunc;
        }

        public Spec<TSubject> Then(Action<TData, TResult> assertion)
        {
            _assertion = assertion;

            var spec = new Spec<TSubject>(Scenario);
            spec.RegisterAssertion(Assert);

            return spec;
        }

        private void Assert(object sender, TSubject subject)
        {
            OnAssert(this, subject);

            _assertion(_data, _resultFunc());
        }
    }
}
