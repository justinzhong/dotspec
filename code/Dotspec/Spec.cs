using System;

namespace Dotspec
{
    /// <summary>
    /// Represents a complete test specification ready to assert.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class Spec<TSubject> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Sole constructor
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="assertionCallback"></param>
        public Spec(string scenario, EventHandler<TSubject> assertionCallback) : base(scenario)
        {
            RegisterAssertionCallback(assertionCallback);
        }

        /// <summary>
        /// Includes an additional assertion to be called when this test 
        /// specification is asserted.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> And(Action<TSubject> assertion)
        {
            RegisterAssertionCallback((_, subject) => assertion(subject));

            return this;
        }

        /// <summary>
        /// Evaluates all the steps included for this test specification.
        /// </summary>
        /// <param name="subject"></param>
        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            // Invokes all previously registered assertion callbacks.
            OnAssert(this, subject);
        }
    }
}
