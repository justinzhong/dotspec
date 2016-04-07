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
    public class ExceptionAssertionSpec<TSubject, TException> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
        where TException : Exception
    {
        private readonly Action<TSubject, TException> _assertion;
        private bool _exceptionAssertedFlag;

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="assertion"></param>
        /// <param name="callback"></param>
        public ExceptionAssertionSpec(string scenario, Action<TSubject, TException> assertion, EventHandler<TSubject> callback) : base(scenario)
        {
            if (assertion == null) throw new ArgumentNullException("assertion");

            _assertion = assertion;

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

        /// <summary>
        /// Callback method for when an exception is raised during assertion.
        /// 
        /// Evaluates the exception related assertion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void OnExceptionCallback(object sender, SpecExceptionArg<TSubject> arg)
        {
            _assertion(arg.Subject, arg.Exception as TException);
            _exceptionAssertedFlag = true;
        }
    }
}
