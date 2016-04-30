using System;

namespace Dotspec
{
    /// <summary>
    /// Provides the Then() test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class AssertionSpec<TSubject> : AssertionSpecBase<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Records the scenario for this test specification.
        /// </summary>
        /// <param name="scenario"></param>
        public AssertionSpec(string scenario) : base(scenario) { }

        /// <summary>
        /// Registers the <paramref name="behaviour"/> action in the assertion 
        /// pipeline and instantiates an AssertionSpec instance.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="behaviour"></param>
        /// <param name="callback"></param>
        public AssertionSpec(string scenario, Action<TSubject> behaviour, EventHandler<TSubject> callback = null) : base(scenario)
        {
            if (behaviour == null) throw new ArgumentNullException("behaviour");

            if (callback != null) RegisterAssertionCallback(callback);

            RegisterAssertionCallback((_, subject) => behaviour(subject));
        }

        /// <summary>
        /// Records the assertion and returns a completed specification.
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> Then(Action<TSubject> assertion)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            // Registers the specified assertion to the OnAssert event chain.
            RegisterAssertionCallback((source, subject) => assertion(subject));

            return SpecFactory.BuildFullSpec(Scenario, OnAssert);
        }
    }
}
