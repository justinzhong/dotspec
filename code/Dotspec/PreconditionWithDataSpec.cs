using System;

namespace Dotspec
{
    /// <summary>
    /// Strong types the input data allowing explicit type to be specified in
    /// test specifications.
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

        public PreconditionSpec<TSubject, TData> And(Action<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition(_data));

            return this;
        }

        /// <summary>
        /// Transitions to AssertionSpec which accepts TData as one of its type
        /// definitions.
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
