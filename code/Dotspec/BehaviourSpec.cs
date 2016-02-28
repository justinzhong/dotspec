using System;

namespace Dotspec
{
    public class BehaviourSpec<TSubject, TData> : SpecBase<TSubject>
        where TSubject : class
    {
        private readonly TData _data;
        private Action<TSubject, TData> _behaviour;

        public BehaviourSpec(string scenario, TData val) : base(scenario)
        {
            _data = val;
        }

        public AssertionSpec<TSubject, TData, TResult> When<TResult>(Func<TSubject, TData, TResult> behaviour)
        {
            // Keeping a variable in heap to hold the result from evaluating the
            // behaviour.
            TResult[] resultContainer = new TResult[1];

            // Return a promise to retrieve the result captured in resultContainer.
            Func<TResult> resultPromise = () => resultContainer[0];

            _behaviour = (subject, data) =>
            {
                var result = behaviour(subject, _data);
                resultContainer[0] = result;
            };

            var spec = new AssertionSpec<TSubject, TData, TResult>(Scenario, _data, resultPromise);
            spec.RegisterAssertion(Assert);

            return spec;
        }

        private void Assert(object sender, TSubject subject)
        {
            // Bubble up the assert event to the parent.
            OnAssert(this, subject);

            _behaviour(subject, _data);
        }
    }
}
