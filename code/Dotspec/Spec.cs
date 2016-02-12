using System;
using System.Collections.Generic;

namespace Dotspec
{
    /// <summary>
    /// Represents a specification for a test scenario.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class Spec<TSubject>
        where TSubject : class
    {
        private bool _assertException;

        private readonly List<Action<SubjectContext<TSubject>>> _givenPreconditions;

        private readonly List<Action<SubjectContext<TSubject>>> _whenBehaviours;

        private readonly List<Action<SubjectContext<TSubject>>> _thenOutcomes;

        private readonly string _scenario;

        /// <summary>
        /// Creates a new Spec test scenario.
        /// </summary>
        /// <param name="scenario"></param>
        public Spec(string scenario)
        {
            if (scenario == null) throw new ArgumentNullException("scenario");

            if (string.IsNullOrEmpty(scenario)) throw new ArgumentException("Parameter cannot be empty string.", "scenario");

            _givenPreconditions = new List<Action<SubjectContext<TSubject>>>();
            _whenBehaviours = new List<Action<SubjectContext<TSubject>>>();
            _thenOutcomes = new List<Action<SubjectContext<TSubject>>>();
            _scenario = scenario;
        }

        /// <summary>
        /// Asserts this Spec test scenario with the subject supplied.
        /// </summary>
        /// <param name="subject"></param>
        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            var context = new SubjectContext<TSubject>(subject);

            RunSteps(context, _givenPreconditions, false);
            RunSteps(context, _whenBehaviours, _assertException);
            RunSteps(context, _thenOutcomes, false);
        }

        /// <summary>
        /// Specifies a precondition (Given) for this test scenario.
        /// 
        /// TODO: Allow optional parameter "message" such that when an exception occured during
        /// performing this step, a user defined message can be displayed.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public Spec<TSubject> Given(Action<SubjectContext<TSubject>> precondition)
        {
            _givenPreconditions.Add(precondition);

            return this;
        }

        /// <summary>
        /// Specifies a behaviour (When) which this test scenario is asserting.
        /// </summary>
        /// <param name="whenBehaviour"></param>
        /// <returns></returns>
        public Spec<TSubject> When(Action<SubjectContext<TSubject>> whenBehaviour)
        {
            _whenBehaviours.Add(whenBehaviour);

            return this;
        }

        /// <summary>
        /// Specifies an outcome (Then) that this test scenario expects.
        /// </summary>
        /// <param name="thenOutcome"></param>
        /// <returns></returns>
        public Spec<TSubject> Then(Action<SubjectContext<TSubject>> thenOutcome)
        {
            _thenOutcomes.Add(thenOutcome);

            return this;
        }

        /// <summary>
        /// Specifies an outcome which involves an exception that this test scenario expects.
        /// </summary>
        /// <param name="thenAssertAction"></param>
        /// <returns></returns>
        public Spec<TSubject> ThenAssertException(Action<SubjectContext<TSubject>> thenAssertAction)
        {
            _assertException = true;

            return Then(thenAssertAction);
        }

        private void RunSteps(SubjectContext<TSubject> context, List<Action<SubjectContext<TSubject>>> steps, bool assertException)
        {
            try
            {
                steps.ForEach(step => step(context));
            }
            catch
            {
                if (!assertException) throw;
            }
        }
    }
}
