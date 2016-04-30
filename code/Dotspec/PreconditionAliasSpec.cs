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
        /// Captures the <paramref name="data"/> parameter to make available for
        /// subsequent precondition or behaviour specifications.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> And<TData>(TData data)
        {
            return SpecFactory.BuildPreconditionAliasWithDataSpec(Scenario, data, OnAssert);
        }

        /// <summary>
        /// Adds the <paramref name="precondition"/> to the list of assertion
        /// callbacks.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionAliasSpec<TSubject> And(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition());

            return this;
        }

        /// <summary>
        /// Adds the <paramref name="precondition"/> to the list of assertion
        /// callbacks.
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
        /// Records the <paramref name="behaviour"/> delegate and transitions to
        /// assertion test specification steps.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpecWithResult<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, behaviour, OnAssert);
        }
    }
}
