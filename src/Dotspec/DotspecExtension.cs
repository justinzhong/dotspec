using Dotspec.Arrange;
using System;

namespace Dotspec
{
    public static class DotspecExtension
    {
        public static ArrangeSpec<TSubject> Spec<TSubject>(this string scenario)
            where TSubject : class
        {
            if (string.IsNullOrWhiteSpace(scenario)) throw new ArgumentException("String cannot be empty.", nameof(scenario));

            return new ArrangeSpec<TSubject>(scenario, new SpecFactory<TSubject>());
        }
    }
}
