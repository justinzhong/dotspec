using System;
using Shouldly;

namespace Dotspec
{
    public class ExceptionSpec<TSubject, TException> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
        where TException : Exception
    {
        private readonly string _exceptionMessage;

        public ExceptionSpec(string scenario, string exceptionMessage) : base(scenario)
        {
            _exceptionMessage = exceptionMessage;
            RegisterOnExceptionCallback(OnExceptionCallback);
        }

        public void Assert(TSubject subject)
        {
            OnAssert(this, subject);
        }

        private void OnExceptionCallback(object sender, SpecExceptionArg<TSubject> arg)
        {
            arg.Exception.ShouldBeOfType<TException>();
            arg.Exception.Message.ShouldBe(_exceptionMessage);
        }
    }
}
