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
        /// Compares the <paramref name="exceptionMessage"/> with the actual 
        /// message contained in the exception that is thrown during the 
        /// assertion of this test specification.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        public Spec<TSubject> Throws<TException>(string exceptionMessage = null)
            where TException : Exception
        {
            return SpecFactory.BuildFullSpec<TException>(Scenario, exceptionMessage, OnAssert);
        }

        /// <summary>
        /// Evaluates the <paramref name="assertion"/> with the actual 
        /// exception that is thrown during the assertion of this test 
        /// specification.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> Throws<TException>(Action<TException> assertion)
            where TException : Exception
        {
            Action<TSubject, TException> assertionWrapper = (_, exception) => assertion(exception);

            return Throws(assertionWrapper);
        }

        /// <summary>
        /// Evaluates the <paramref name="assertion"/> with the test subject
        /// and the actual exception that is thrown during the assertion of this
        /// test specification.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="assertion"></param>
        /// <returns></returns>
        public Spec<TSubject> Throws<TException>(Action<TSubject, TException> assertion)
            where TException : Exception
        {
            return SpecFactory.BuildFullSpec(Scenario, assertion, OnAssert);
        }
    }
}
