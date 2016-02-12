using System;
using System.Collections.Generic;

namespace Dotspec
{
    /// <summary>
    /// Represents the context for a test scenario.
    /// 
    /// This includes the test subject and any test states that is required to
    /// be carried through the various test steps.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class SubjectContext<TSubject>
        where TSubject : class
    {
        private readonly Dictionary<string, object> _parameters;

        /// <summary>
        /// The subject under test (SUT).
        /// </summary>
        public readonly TSubject Subject;

        /// <summary>
        /// The state of a test scenario.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return _parameters[key];
            }

            set
            {
                _parameters[key] = value;
            }
        }

        /// <summary>
        /// Instantiates a new SubjectContext for the supplied <paramref name="subject"/>.
        /// </summary>
        /// <param name="subject"></param>
        public SubjectContext(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            Subject = subject;
            _parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Retrieves a test data for this test scenario, based on the <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TVal GetVal<TVal>(string key)
        {
            return (TVal)_parameters[key];
        }
    }
}
