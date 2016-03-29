using System;
using Shouldly;

namespace Dotspec
{
    public class ExceptionAssertionSpec<TSubject, TException> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
        where TException : Exception
    {
        private readonly Action<TSubject, TException> _assertion;
        private bool _exceptionAssertedFlag;

        public ExceptionAssertionSpec(string scenario, Action<TSubject, TException> assertion, EventHandler<TSubject> callback) : base(scenario)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            _assertion = assertion;

            RegisterAssertionCallback(callback);
            RegisterOnExceptionCallback(OnExceptionCallback);
        }

        public void Assert(TSubject subject)
        {
            OnAssert(this, subject);

            _exceptionAssertedFlag.ShouldBeTrue();
        }

        private void OnExceptionCallback(object sender, SpecExceptionArg<TSubject> arg)
        {
            _assertion(arg.Subject, arg.Exception as TException);
            _exceptionAssertedFlag = true;
        }
    }
}
