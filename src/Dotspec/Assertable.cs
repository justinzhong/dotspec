using System;
using System.Collections.Generic;

namespace Dotspec
{
    public class Assertable<TSubject> : IAssertable<TSubject>
        where TSubject : class
    {
        private Action<TSubject> AssertionSpec { get; }

        public Assertable(Action<TSubject> assertionSpec)
        {
            if (assertionSpec == null) throw new ArgumentNullException(nameof(assertionSpec));

            AssertionSpec = assertionSpec;
        }

        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            AssertionSpec(subject);
        }
    }
}