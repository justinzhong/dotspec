using System;

namespace Dotspec
{
    /// <summary>
    /// Represents a fully specified test specification ready for assertion.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class Spec<TSubject, TData> : Spec<TSubject>
        where TSubject : class
    {
        private readonly TData _data;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="assertionCallback"></param>
        /// <param name="data"></param>
        public Spec(string scenario, EventHandler<TSubject> assertionCallback, TData data) : base(scenario, assertionCallback)
        {
            _data = data;
        }

        /// <summary>
        /// Adds the <paramref name="assertion"/> step to the AssertionEvent 
        /// event call stack.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject, TData> And(Action<TSubject, TData> assertion)
        {
            RegisterAssertionCallback((_, subject) => assertion(subject, _data));

            return this;
        }
    }
}
