using System;

namespace Dotspec
{
    public interface ISubjectConstructorSpec<TSubject, TData> where TSubject : class
    {
        void For(Func<TData, TSubject> subjectConstructor);
    }
}