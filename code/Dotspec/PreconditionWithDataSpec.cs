using System;

namespace Dotspec
{
    /// <summary>
    /// Records the preconditions and input for a test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class PreconditionSpec<TSubject, TData> : PreconditionSpec<TSubject>
        where TSubject : class
    {
        private readonly TData _data;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="data"></param>
        public PreconditionSpec(string scenario, TData data) : base(scenario)
        {
            if (data == null) throw new ArgumentNullException("data");

            _data = data;
        }

        /// <summary>
        /// Given a behaviour, returns the assertion step of the test 
        /// specification.
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
