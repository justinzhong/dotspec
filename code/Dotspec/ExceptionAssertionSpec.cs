using System;

namespace Dotspec
{
    public class ExceptionAssertionSpec<TSubject, TException> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
        where TException : Exception
    {
        private readonly Action<TSubject, TException> _assertion;

        public ExceptionAssertionSpec(string scenario, Action<TSubject, TException> assertion) : base(scenario)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            _assertion = assertion;
            RegisterOnExceptionCallback(OnExceptionCallback);
        }

        public void Assert(TSubject subject)
        {
            OnAssert(this, subject);
        }

        private void OnExceptionCallback(object sender, SpecExceptionArg<TSubject> arg)
        {
            _assertion(arg.Subject, arg.Exception as TException);
        }
    }
}
