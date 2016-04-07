using System;

namespace Dotspec
{
    /// <summary>
    /// Provides the Given() and When() test specification providing a TData
    /// strong typed parameter to be used in those steps.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class PreconditionSpec<TSubject, TData> : PreconditionAliasSpec<TSubject>
        where TSubject : class
    {
        private readonly TData _data;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="data"></param>
        public PreconditionSpec(string scenario, TData data, EventHandler<TSubject> callback = null) : base(scenario)
        {
            if (data == null) throw new ArgumentNullException("data");

            _data = data;

            if (callback != null) RegisterAssertionCallback(callback);
        }

        /// <summary>
        /// Captures <paramref name="precondition"/> delegate to be evaluated at
        /// the time of asserting this test specification.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> And(Action<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition(_data));

            return this;
        }

        /// <summary>
        /// Captures the <paramref name="behaviour"/> delegate which yields the
        /// result type TResult and start provide Then() test specification steps.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject, TData, TResult> When<TResult>(Func<TSubject, TData, TResult> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, _data, behaviour, OnAssert);
        }
    }
}
