using System.Collections;
using System.Collections.Generic;

namespace Dotspec
{
    public class Specs<TSubject> : IEnumerable<IAssertableSpec<TSubject>>, IEnumerator<IAssertableSpec<TSubject>>
        where TSubject : class
    {
        private readonly List<IAssertableSpec<TSubject>> _container;

        public Specs()
        {
            _container = new List<IAssertableSpec<TSubject>>();
        }

        public IAssertableSpec<TSubject> Current
        {
            get
            {
                return GetEnumerator().Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            GetEnumerator().Dispose();
        }

        public IEnumerator<IAssertableSpec<TSubject>> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        public bool MoveNext()
        {
            return GetEnumerator().MoveNext();
        }

        public void Reset()
        {
            GetEnumerator().Reset();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
