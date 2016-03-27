using System;

namespace Dotspec
{
    public class Spec<TSubject> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
    {
        private readonly Action<TSubject> _assertion;

        public Spec(string scenario, Action<TSubject> assertion) : base(scenario)
        {
            _assertion = assertion;
        }

        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            // Invokes all previously registered assertion callbacks.
            OnAssert(this, subject);

            // Perform the actual assertion on the test subject.
            _assertion(subject);
        }
    }
}
