using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotspec
{
    /// <summary>
    /// Provides extension methods for instantiating and asserting Spec test 
    /// scenario.
    /// </summary>
    public static class SpecExtension
    {
        /// <summary>
        /// Instantiates a new Spec test scenario.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public static Spec<T> Spec<T>(this string scenario)
            where T : class
        {
            if (scenario == null) throw new ArgumentNullException("scenario");

            if (string.IsNullOrEmpty(scenario)) throw new ArgumentException("Parameter cannot be empty string.", "scenario");

            return new Spec<T>(scenario);
        }

        /// <summary>
        /// Asserts multiple Spec test scenarios.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specs"></param>
        /// <param name="subject"></param>
        public static void Assert<T>(this IEnumerable<Spec<T>> specs, T subject)
            where T : class
        {
            if (subject == null) throw new ArgumentNullException("subject");

            specs.ToList().ForEach(spec => spec.Assert(subject));
        }
    }
}
