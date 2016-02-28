using System;

namespace Dotspec
{
    public class Spec<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        public Spec(string scenario) : base(scenario) { }

        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            OnAssert(this, subject);
        }
    }
}
