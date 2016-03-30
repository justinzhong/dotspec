using System;

namespace Dotspec
{
    public class Spec<TSubject, TData> : Spec<TSubject>
        where TSubject : class
    {
        private readonly TData _data;

        public Spec(string scenario, EventHandler<TSubject> assertionCallback, TData data) : base(scenario, assertionCallback)
        {
            _data = data;
        }

        public Spec<TSubject, TData> And(Action<TSubject, TData> assertion)
        {
            RegisterAssertionCallback((_, subject) => assertion(subject, _data));

            return this;
        }
    }
}
