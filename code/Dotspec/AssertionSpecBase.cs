using System;

namespace Dotspec
{
    /// <summary>
    /// Base class for AssertionSpec implementations which provides the Then() 
    /// test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public abstract class AssertionSpecBase<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        public AssertionSpecBase(string scenario) : base(scenario) { }

        /// <summary>
        /// Transitions to full spec object to assert the specified exception 
        /// type with the optional exception message.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        public Spec<TSubject> Throws<TException>(string exceptionMessage = null)
            where TException : Exception
        {
            return SpecFactory.BuildFullSpec<TException>(Scenario, exceptionMessage, OnAssert);
        }
    }
}
