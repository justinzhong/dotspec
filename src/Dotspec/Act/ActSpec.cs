using System;

namespace Dotspec
{
    /// <summary>
    /// Provides specification for the behaviour of a test spec.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class ActSpec<TSubject> : IActSpec<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }

        private ISpecFactory<TSubject> SpecFactory { get; }

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="precondition"></param>
        /// <param name="specFactory"></param>
        public ActSpec(Action precondition, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            SpecFactory = specFactory;
        }

        /// <summary>
        /// Given the behaviour to assert for this test spec, return the next step, 'Then'.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public IAssertionSpec<TSubject> When(Action<TSubject> behaviour)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return SpecFactory.CreateAssertionSpec(Precondition, behaviour);
        }

        /// <summary>
        /// Given the behaviour to assert for this test spec, return the next step, 'Then'.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public IResultAssertionSpec<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return SpecFactory.CreateResultAssertionSpec(Precondition, behaviour);
        }
    }
}