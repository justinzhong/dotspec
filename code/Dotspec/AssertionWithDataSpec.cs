using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject, TData, TResult> : AssertionSpecBase<TSubject>
        where TSubject : class
    {
        private readonly TData _data;
        private readonly Func<TSubject, TData, TResult> _behaviour;

        public AssertionSpec(string scenario, TData val, Func<TSubject, TData, TResult> behaviour) : base(scenario)
        {
            _data = val;
            _behaviour = behaviour;
        }

        public Spec<TSubject> Then(Action<TData, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            Action<TSubject> assertionWrapper = (subject) => assertion(_data, _behaviour(subject, _data));

            return SpecFactory.BuildFullSpec(Scenario, assertionWrapper, OnAssert);
        }
    }
}
