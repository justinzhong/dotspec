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
        /// Runs each completely defined FullSpec test scenario with the provided subject.
        /// </summary>
        /// <typeparam name="TSubject"></typeparam>
        /// <param name="specs"></param>
        /// <param name="subject"></param>
        public static void Assert<TSubject>(this IEnumerable<Spec<TSubject>> specs, TSubject subject)
            where TSubject : class
        {
            specs.All(spec =>
            {
                spec.Assert(subject);
                return true;
            });
        }

        /// <summary>
        /// Instantiates a new Spec test scenario, starting with a precondition specification.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public static PreconditionSpec<T> Spec<T>(this string scenario)
            where T : class
        {
            if (scenario == null) throw new ArgumentNullException("scenario");

            if (string.IsNullOrEmpty(scenario)) throw new ArgumentException("Parameter cannot be empty string.", "scenario");

            return new PreconditionSpec<T>(scenario);
        }
    }
}
