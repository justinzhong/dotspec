using System;

namespace Dotspec
{
    public class SpecFactory<TSubject>
        where TSubject : class
    {
        public AssertionSpec<TSubject, TResult> BuildAssertionSpec<TResult>(
            string scenario, Func<TSubject, TResult> behaviour, EventHandler<TSubject> callback)
        {
            var assertionSpec = new AssertionSpec<TSubject, TResult>(scenario, behaviour);
            assertionSpec.RegisterAssertionCallback(callback);

            return assertionSpec;
        }


        public AssertionSpec<TSubject, TData, TResult> BuildAssertionSpec<TData, TResult>(
            string scenario, TData data, Func<TSubject, TData, TResult> behaviour, EventHandler<TSubject> callback)
        {
            var assertionSpec = new AssertionSpec<TSubject, TData, TResult>(scenario, data, behaviour);
            assertionSpec.RegisterAssertionCallback(callback);

            return assertionSpec;
        }

        public ExceptionSpec<TSubject, TException> BuildExceptionSpec<TException>(
            string scenario, string exceptionMessage, EventHandler<TSubject> callback)
            where TException : Exception
        {
            var exceptionSpec = new ExceptionSpec<TSubject, TException>(scenario, exceptionMessage);
            exceptionSpec.RegisterAssertionCallback(callback);

            return exceptionSpec;
        }

        public Spec<TSubject> BuildFullSpec(string scenario, Action<TSubject> assertion, EventHandler<TSubject> callback)
        {
            var spec = new Spec<TSubject>(scenario, assertion);
            spec.RegisterAssertionCallback(callback);

            return spec;
        }

        public PreconditionSpec<TSubject, TData> BuildPreconditionWithDataSpec<TData>(
            string scenario, TData data, EventHandler<TSubject> callback)
        {
            var preconditionSpec = new PreconditionSpec<TSubject, TData>(scenario, data);
            preconditionSpec.RegisterAssertionCallback(callback);

            return preconditionSpec;
        }
    }
}
