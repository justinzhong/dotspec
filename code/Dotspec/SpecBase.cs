using System;

namespace Dotspec
{
    /// <summary>
    /// Represents the base common functions of a test specification.
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

        public void RegisterAssertionCallback(EventHandler<TSubject> assertionCallback)
        {
            AssertEvent += assertionCallback;
        }

        public void RegisterOnExceptionCallback(EventHandler<SpecExceptionArg<TSubject>> onExceptionCallback)
        {
            OnExceptionEvent += onExceptionCallback;
        }

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

    public class SpecExceptionArg<TSubject>
        where TSubject : class
    {
        public TSubject Subject { get; set; }

        public Exception Exception { get; set; }
    }
}
