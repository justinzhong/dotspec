using System;
using Shouldly;

namespace Dotspec
{
    /// <summary>
    /// Responsible for registering any exceptions-related assertions and their 
    /// evaluation during the assertion of the test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    /// <typeparam name="TException"></typeparam>
    public class ExceptionSpec<TSubject, TException> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
        where TException : Exception
    {
        private readonly string _exceptionMessage;
        private bool _exceptionAssertedFlag;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="callback"></param>
        public ExceptionSpec(string scenario, string exceptionMessage, EventHandler<TSubject> callback) : base(scenario)
        {
            _exceptionMessage = exceptionMessage;

            RegisterAssertionCallback(callback);
            RegisterOnExceptionCallback(OnExceptionCallback);
        }

        /// <summary>
        /// Asserts the test specifications for the intended subject <paramref name="TSubject"/>
        /// 
        /// Checks exceptions-related assertions have been evaluated.
        /// </summary>
        /// <param name="subject"></param>
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
