using System;

namespace Dotspec
{
    /// <summary>
    /// Alias for PreconditionSpec, providing method name And() in place of 
    /// Given() for syntactic sugar.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class PreconditionAliasSpec<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        public PreconditionAliasSpec(string scenario) : base(scenario) { }

        /// <summary>
        /// Transitions to PreconditionSpec object which accepts TData as one of
        /// its type definitions.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> And<TData>(TData data)
        {
            return SpecFactory.BuildPreconditionAliasWithDataSpec(Scenario, data, OnAssert);
        }

        public PreconditionAliasSpec<TSubject> And(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition());

            return this;
        }

        /// <summary>
        /// Records a precondition for this test specification.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionAliasSpec<TSubject> And(Action<TSubject> precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition(subject));

            return this;
        }

        /// <summary>
        /// Transitions to AssertionSpec which captures the specified behaviour.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, behaviour, OnAssert);
        }
    }
}
