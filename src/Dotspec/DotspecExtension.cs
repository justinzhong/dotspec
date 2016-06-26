using System;

namespace Dotspec
{
    public static class DotspecExtension
    {
        public static PreconditionSpec<TSubject> Spec<TSubject>(this string scenario)
            where TSubject : class
        {
            if (string.IsNullOrWhiteSpace(scenario)) throw new ArgumentException("String cannot be empty.", nameof(scenario));

            return new PreconditionSpec<TSubject>(scenario, CreateSpecFactory<TSubject>());
        }

        private static ISpecFactory<TSubject> CreateSpecFactory<TSubject>()
            where TSubject : class
        {
            return new SpecFactory<TSubject>(precondition => new BehaviourSpec<TSubject>(precondition));
        }
    }
}
