using System;

namespace Dotspec
{
    /// <summary>
    /// Captures the exception thrown during assertion of a test specification
    /// on the <typeparamref name="TSubject"/> subject.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class SpecExceptionArg<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// The subject of the test specification.
        /// </summary>
        public TSubject Subject { get; set; }

        /// <summary>
        /// The exception thrown during assertion of a test specification.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
