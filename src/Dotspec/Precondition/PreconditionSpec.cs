using System;

namespace Dotspec
{
    public class PreconditionSpec<TSubject> : IPreconditionSpec<TSubject>
        where TSubject : class
    {
        private string Scenario { get; }

        private ISpecFactory<TSubject> SpecFactory { get; }

        public PreconditionSpec(string scenario, ISpecFactory<TSubject> specFactory)
        {
            if (string.IsNullOrWhiteSpace(scenario)) throw new ArgumentException($"String cannot be empty.", nameof(scenario));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Scenario = scenario;
            SpecFactory = specFactory;
        }

        public IBehaviourSpec<TSubject> Given(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return SpecFactory.CreateBehaviourSpec(precondition);
        }

        public IBehaviourSpec<TSubject, TData> Given<TData>(Func<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return SpecFactory.CreateBehaviourSpec(precondition);
        }
    }
}