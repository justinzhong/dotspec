using System;

namespace Dotspec.Arrange
{
    /// <summary>
    /// Provides specification for the precondition of a test spec.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public interface IArrangeSpec<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Given a precondition for this test spec, return the next step, 'When'.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        IActSpec<TSubject> Given(Action precondition);

        /// <summary>
        /// Given a precondition for this test spec, return the next step, 'When'.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="precondition"></param>
        /// <returns></returns>
        IActSpec<TSubject, TData> Given<TData>(Func<TData> precondition);
    }
}
