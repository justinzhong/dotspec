using System;

namespace Dotspec.Arrange
{
    /// <summary>
    /// Provides specification for the precondition of a test spec.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class ArrangeSpec<TSubject> : IArrangeSpec<TSubject>
        where TSubject : class
    {
        private string Scenario { get; }

        private ISpecFactory<TSubject> SpecFactory { get; }

        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="specFactory"></param>
        public ArrangeSpec(string scenario, ISpecFactory<TSubject> specFactory)
        {
            if (string.IsNullOrWhiteSpace(scenario)) throw new ArgumentException($"String cannot be empty.", nameof(scenario));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Scenario = scenario;
            SpecFactory = specFactory;
        }

        /// <summary>
        /// Given a precondition for this test spec, return the next step, 'When'.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public IActSpec<TSubject> Given(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return SpecFactory.CreateBehaviourSpec(precondition);
        }

        /// <summary>
        /// Given a precondition for this test spec, return the next step, 'When'.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public IActSpec<TSubject, TData> Given<TData>(Func<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return SpecFactory.CreateBehaviourSpec(precondition);
        }
    }
}