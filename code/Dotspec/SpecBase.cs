using System;

namespace Dotspec
{
    /// <summary>
    /// Represents a specification for a test scenario.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public abstract class SpecBase<TSubject> : IAssertEventObserver<TSubject>
        where TSubject : class
    {
        public event EventHandler<TSubject> AssertEvent;

        public event EventHandler<TSubject> UnsubscribeEvent;

        public string Scenario { get; protected set; }

        protected readonly SpecFactory<TSubject> SpecFactory;

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
            UnsubscribeEvent += (_, subject) => AssertEvent -= assertionCallback;
        }

        protected void OnAssert(object source, TSubject subject)
        {
            var eventHandler = AssertEvent;

            if (eventHandler != null)
            {
                eventHandler(source, subject);
            }
        }

        protected void OnUnsubscribe(object source, TSubject subject)
        {
            var eventHandler = UnsubscribeEvent;

            if (eventHandler != null)
            {
                eventHandler(source, subject);
            }
        }
    }
}
