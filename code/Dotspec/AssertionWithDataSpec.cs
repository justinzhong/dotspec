using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject, TData, TResult> : AssertionSpecBase<TSubject>
        where TSubject : class
    {
        private readonly TData _data;
        private EventHandler<TResult> ResultEvent;

        public AssertionSpec(string scenario, TData val, Func<TSubject, TData, TResult> behaviour, EventHandler<TSubject> callback = null) : base(scenario)
        {
            _data = val;

            if (callback != null) RegisterAssertionCallback(callback);

            RegisterAssertionCallback((_, subject) =>
            {
                var result = behaviour(subject, _data);
                var resultEventHandler = ResultEvent;

                if (resultEventHandler != null) resultEventHandler(this, result);
            });
        }

        public Spec<TSubject> Then(Action<TSubject, TData, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            // Registers the specified assertion to the OnAssert event chain.
            RegisterAssertionCallback((source, subject) => RegisterResultCallback(result => assertion(subject, _data, result)));

            return SpecFactory.BuildFullSpec(Scenario, OnAssert);
        }

        /// <summary>
        /// Registers a callback to the ResultEvent to capture and use the 
        /// result in the assertion.
        /// </summary>
        /// <param name="resultAssertion"></param>
        protected void RegisterResultCallback(Action<TResult> resultAssertion)
        {
            ResultEvent += (_, result) => resultAssertion(result);
        }
    }
}
