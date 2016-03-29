using System;

namespace Dotspec
{
    public class SpecFactory<TSubject>
        where TSubject : class
    {
        public AssertionSpec<TSubject, TResult> BuildAssertionSpec<TResult>(
            string scenario, Func<TSubject, TResult> behaviour, EventHandler<TSubject> callback)
        {
            return new AssertionSpec<TSubject, TResult>(scenario, behaviour, callback);
        }

        public AssertionSpec<TSubject, TData, TResult> BuildAssertionSpec<TData, TResult>(
            string scenario, TData data, Func<TSubject, TData, TResult> behaviour, EventHandler<TSubject> callback)
        {
            return new AssertionSpec<TSubject, TData, TResult>(scenario, data, behaviour, callback);
        }

        public Spec<TSubject> BuildFullSpec<TException>(
            string scenario, Action<TSubject, TException> assertion, EventHandler<TSubject> callback)
            where TException : Exception
        {
            var exceptionSpec = new ExceptionAssertionSpec<TSubject, TException>(scenario, assertion, callback);

            return new Spec<TSubject>(scenario, (_, subject) => exceptionSpec.Assert(subject));
        }

        public Spec<TSubject> BuildFullSpec<TException>(
            string scenario, string exceptionMessage, EventHandler<TSubject> callback)
            where TException : Exception
        {
            var exceptionSpec = new ExceptionSpec<TSubject, TException>(scenario, exceptionMessage, callback);

            return new Spec<TSubject>(scenario, (_, subject) => exceptionSpec.Assert(subject));
        }

        public Spec<TSubject> BuildFullSpec(string scenario, EventHandler<TSubject> callback)
        {
            return new Spec<TSubject>(scenario, callback);
        }

        public PreconditionSpec<TSubject, TData> BuildPreconditionWithDataSpec<TData>(
            string scenario, TData data, EventHandler<TSubject> callback)
        {
            return new PreconditionSpec<TSubject, TData>(scenario, data, callback);
        }
    }
}
