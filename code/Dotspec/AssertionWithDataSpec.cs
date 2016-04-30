using System;

namespace Dotspec
{
    /// <summary>
    /// Provides the Then() test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class AssertionSpec<TSubject, TData, TResult> : AssertionSpecWithResult<TSubject, TResult>
        where TSubject : class
    {
        private readonly TData _data;

        /// <summary>
        /// Sole constructor
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="val"></param>
        /// <param name="behaviour"></param>
        /// <param name="callback"></param>
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

        /// <summary>
        /// Takes the <paramref name="assertion"/> delegate which works with 
        /// both TData and TResult parameters and returns a fully specified Spec
        /// object that is ready to be asserted.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject, TData> Then(Action<TData, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            // Registers the specified assertion to the OnAssert event chain.
            RegisterAssertionCallback((source, subject) => RegisterResultCallback(result => assertion(_data, result)));

            return SpecFactory.BuildFullSpec(Scenario, OnAssert, _data);
        }

        /// <summary>
        /// Takes the <paramref name="assertion"/> delegate which works with 
        /// TSubject, TData and TResult and returns a fully specified Spec 
        /// object that is ready to be asserted.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> Then(Action<TSubject, TData, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            // Registers the specified assertion to the OnAssert event chain.
            RegisterAssertionCallback((source, subject) => RegisterResultCallback(result => assertion(subject, _data, result)));

            return SpecFactory.BuildFullSpec(Scenario, OnAssert, _data);
        }
    }
}
