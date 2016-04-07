using System;

namespace Dotspec
{
    /// <summary>
    /// Responsible for constructing different test specification classes,
    /// such as: precondition, assertion, exception and full spec object.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class SpecFactory<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Builds an assertion spec for recording:
        /// 1) Behaviour actions via When() specifications
        /// 2) Assertion actions via Then() specifications
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="scenario"></param>
        /// <param name="behaviour"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject, TResult> BuildAssertionSpec<TResult>(
            string scenario, Func<TSubject, TResult> behaviour, EventHandler<TSubject> callback)
        {
            return new AssertionSpec<TSubject, TResult>(scenario, behaviour, callback);
        }

        /// <summary>
        /// Builds an assertion spec for recording When() and Then() specifications.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="scenario"></param>
        /// <param name="data"></param>
        /// <param name="behaviour"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject, TData, TResult> BuildAssertionSpec<TData, TResult>(
            string scenario, TData data, Func<TSubject, TData, TResult> behaviour, EventHandler<TSubject> callback)
        {
            return new AssertionSpec<TSubject, TData, TResult>(scenario, data, behaviour, callback);
        }

        /// <summary>
        /// Builds the full specification ready for assertion.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="scenario"></param>
        /// <param name="assertion"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Spec<TSubject> BuildFullSpec<TException>(
            string scenario, Action<TSubject, TException> assertion, EventHandler<TSubject> callback)
            where TException : Exception
        {
            var exceptionSpec = new ExceptionAssertionSpec<TSubject, TException>(scenario, assertion, callback);

            return new Spec<TSubject>(scenario, (_, subject) => exceptionSpec.Assert(subject));
        }

        /// <summary>
        /// Builds the full specification ready for assertion.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="scenario"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Spec<TSubject> BuildFullSpec<TException>(
            string scenario, string exceptionMessage, EventHandler<TSubject> callback)
            where TException : Exception
        {
            var exceptionSpec = new ExceptionSpec<TSubject, TException>(scenario, exceptionMessage, callback);

            return new Spec<TSubject>(scenario, (_, subject) => exceptionSpec.Assert(subject));
        }

        /// <summary>
        /// Builds the full specification ready for assertion.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Spec<TSubject> BuildFullSpec(string scenario, EventHandler<TSubject> callback)
        {
            return new Spec<TSubject>(scenario, callback);
        }

        /// <summary>
        /// Builds the full specification ready for assertion.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="scenario"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Spec<TSubject, TData> BuildFullSpec<TData>(string scenario, EventHandler<TSubject> callback, TData data)
        {
            return new Spec<TSubject, TData>(scenario, callback, data);
        }

        /// <summary>
        /// Builds the precondition spec for recording:
        /// 1) Additional precondition actions via And() specifications
        /// 2) Behaviour actiosn via When() specifications
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="scenario"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> BuildPreconditionAliasWithDataSpec<TData>(
            string scenario, TData data, EventHandler<TSubject> callback)
        {
            return new PreconditionSpec<TSubject, TData>(scenario, data, callback);
        }
    }
}
