using System;

namespace Dotspec
{
    /// <summary>
    /// Provides the Then() test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class AssertionSpecWithResult<TSubject, TResult> : AssertionSpecBase<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// This event is raised when the result is evaluated from the behaviour
        /// provided.
        /// </summary>
        protected EventHandler<TResult> ResultEvent;

        /// <summary>
        /// Records the scenario for this test specification.
        /// </summary>
        /// <param name="scenario"></param>
        public AssertionSpecWithResult(string scenario) : base(scenario) { }

        /// <summary>
        /// In addition to recording the scenario, register the behaviour to be 
        /// executed during assertion and register any callbacks for the 
        /// OnAssert() event chain.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="behaviour"></param>
        /// <param name="callback"></param>
        public AssertionSpecWithResult(string scenario, Func<TSubject, TResult> behaviour, EventHandler<TSubject> callback = null) : base(scenario)
        {
            if (behaviour == null) throw new ArgumentNullException("behaviour");

            if (callback != null) RegisterAssertionCallback(callback);

            RegisterAssertionCallback((_, subject) =>
            {
                var result = behaviour(subject);
                var resultEventHandler = ResultEvent;

                if (resultEventHandler != null) resultEventHandler(this, result);
            });
        }

        /// <summary>
        /// Records the assertion and returns a completed specification.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> Then(Action<TSubject, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            // Registers the specified assertion to the OnAssert event chain.
            RegisterAssertionCallback((source, subject) => RegisterResultCallback(result => assertion(subject, result)));

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
