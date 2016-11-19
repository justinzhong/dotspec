using System;

namespace Dotspec
{
    public class ResultAssertionSpec<TSubject, TData, TResult>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }
        private Func<TSubject, TData, TResult> Behaviour { get; }
        private ISpecFactory<TSubject> SpecFactory { get; }

        public ResultAssertionSpec(Func<TData> precondition, Func<TSubject, TData, TResult> behaviour, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            Behaviour = behaviour;
            SpecFactory = specFactory;
        }
    }
}