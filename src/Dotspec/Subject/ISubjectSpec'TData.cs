using System;

namespace Dotspec
{
    public interface ISubjectSpec<TSubject, TData>
        where TSubject : class
    {
        void For(TSubject subject);

        void For(Func<TData, TSubject> subjectConstructor);
    }
}