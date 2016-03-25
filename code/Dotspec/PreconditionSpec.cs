using System;
using System.Collections.Generic;

namespace Dotspec
{
    /// <summary>
    /// Records the preconditions and input for a test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class PreconditionSpec<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        private List<Action<TSubject>> _preconditions;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        public PreconditionSpec(string scenario) : base(scenario)
        {
            _preconditions = new List<Action<TSubject>>();
        }

        /// <summary>
        /// Alias for Given().
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject> And(Action<TSubject> precondition)
        {
            Given(precondition);

            return this;
        }

        /// <summary>
        /// Records an input for this test specification.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> Given<TData>(TData data)
        {
            return SpecFactory.BuildPreconditionSpec(Scenario, data, Assert);
        }

        /// <summary>
        /// Records a precondition for this test specification.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject> Given(Action<TSubject> precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            _preconditions.Add(precondition);

            return this;
        }

        /// <summary>
        /// Returns an assertable spec using the specified behaviour.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, behaviour, Assert);
        }

        private void Assert(object sender, TSubject subject)
        {
            // Bubble up the assert event to the parent.
            OnAssert(this, subject);

            _preconditions.ForEach(precondition => precondition(subject));
        }
    }
}
