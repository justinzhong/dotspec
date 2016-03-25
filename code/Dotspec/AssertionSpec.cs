using System;

namespace Dotspec
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class AssertionSpec<TSubject, TResult> : SpecBase<TSubject>
        where TSubject : class
    {
        private readonly Func<TSubject, TResult> _behaviour;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="input"></param>
        public AssertionSpec(string scenario, Func<TSubject, TResult> behaviour) : base(scenario)
        {
            if (behaviour == null) throw new ArgumentNullException("behaviour");

            _behaviour = behaviour;
        }

        /// <summary>
        /// Records the assertion and returns a completed specification.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> Then(Action<TSubject, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            Action<TSubject> assertionWrapper = (subject) => assertion(subject, _behaviour(subject));

            return SpecFactory.BuildFullSpec(Scenario, assertionWrapper, OnAssert);
        }
    }
}
