using System;
using Shouldly;

namespace Dotspec
{
    public class ExceptionSpec<TSubject, TException> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
        where TException : Exception
    {
        private readonly string _exceptionMessage;
        private bool _exceptionAssertedFlag;

        public ExceptionSpec(string scenario, string exceptionMessage, EventHandler<TSubject> callback) : base(scenario)
        {
            _exceptionMessage = exceptionMessage;

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
            arg.Exception.ShouldBeOfType<TException>();

            if (!string.IsNullOrEmpty(_exceptionMessage)) arg.Exception.Message.ShouldBe(_exceptionMessage);

            _exceptionAssertedFlag = true;
        }
    }
}
