using System;

namespace Dotspec
{
    /// <summary>
    /// Defines the base functionalities of a test specification with event
    /// handling for assertion and exception.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public abstract class SpecBase<TSubject>
        where TSubject : class
    {
        private event EventHandler<TSubject> AssertEvent;

        private event EventHandler<SpecExceptionArg<TSubject>> OnExceptionEvent;

        protected readonly SpecFactory<TSubject> SpecFactory;

        public string Scenario { get; protected set; }

        /// <summary>
        /// Creates a new Spec test scenario.
        /// </summary>
        /// <param name="scenario"></param>
        public SpecBase(string scenario)
        {
            if (scenario == null) throw new ArgumentNullException("scenario");

            if (string.IsNullOrEmpty(scenario)) throw new ArgumentException("String cannot be empty.", "scenario");

            Scenario = scenario;
            SpecFactory = new SpecFactory<TSubject>();
        }

        /// <summary>
        /// Registers a callback to be invoked during assertion, in the order in
        /// which its registered.
        /// </summary>
        /// <param name="assertionCallback"></param>
        protected void RegisterAssertionCallback(EventHandler<TSubject> assertionCallback)
        {
            AssertEvent += assertionCallback;
        }

        /// <summary>
        /// Registers a callback to be invoked when encountering an exception 
        /// during the assertion.
        /// </summary>
        /// <param name="onExceptionCallback"></param>
        protected void RegisterOnExceptionCallback(EventHandler<SpecExceptionArg<TSubject>> onExceptionCallback)
        {
            OnExceptionEvent += onExceptionCallback;
        }

        /// <summary>
        /// Raises the AssertEvent to invoke all registered callback handlers.
        /// 
        /// When an exception is encountered, passes it to any callback that has
        /// been registered with OnExceptionEvent.
        /// 
        /// If there are no callbacks registered to the OnExceptionEvent, the 
        /// exception is thrown verbatim.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="subject"></param>
        protected void OnAssert(object source, TSubject subject)
        {
            var eventHandler = AssertEvent;

            if (eventHandler != null)
            {
                try
                {
                    eventHandler(source, subject);
                }
                catch (Exception ex)
                {
                    if (OnExceptionEvent == null) throw;

                    OnExceptionEvent(this, new SpecExceptionArg<TSubject> { Exception = ex, Subject = subject });
                }
            }
        }
    }
}
