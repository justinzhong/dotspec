using System;

namespace Dotspec
{
    /// <summary>
    /// Provides the Given() and When() test specifications.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class PreconditionSpec<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        public PreconditionSpec(string scenario) : base(scenario) { }

        /// <summary>
        /// Captures <paramref name="data"/> to be used in subsequent steps,
        /// e.g. 'when', 'then'.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> Given<TData>(TData data)
        {
            return SpecFactory.BuildPreconditionAliasWithDataSpec(Scenario, data, OnAssert);
        }

        /// <summary>
        /// Takes <paramref name="data"/> and bind with 
        /// <paramref name="dataTemplate"/> to create the final data object to
        /// be used in subsequent steps,
        /// e.g. 'when', 'then'.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TData2"></typeparam>
        /// <param name="data"></param>
        /// <param name="dataTemplate"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData2> Given<TData, TData2>(TData data, Func<TData, TData2> dataTemplate)
        {
            return SpecFactory.BuildPreconditionAliasWithDataSpec(Scenario, dataTemplate(data), OnAssert);
        }

        /// <summary>
        /// Captures <paramref name="precondition"/> delegate to be evaluated at
        /// the time of asserting this test specification.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject> Given(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition());

            return this;
        }

        /// <summary>
        /// Captures <paramref name="precondition"/> delegate to be evaluated at
        /// the time of asserting this test specification.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject> Given(Action<TSubject> precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition(subject));

            return this;
        }

        /// <summary>
        /// Captures the <paramref name="behaviour"/> delegate which yields the
        /// result type TResult and start provide Then() test specification steps.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpecWithResult<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, behaviour, OnAssert);
        }

        /// <summary>
        /// Registers the <paramref name="behaviour"/> action into the specification
        /// pipeline for the 'When' step.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject> When(Action<TSubject> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, behaviour, OnAssert);
        }
    }
}
