using System;
using Shouldly;

namespace Dotspec
{
    public class Dotspec<TSubject, TData> : Dotspec<TSubject>, IDotspec<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> DataConstructor { get; set; }

        private event Action<TSubject, TData> OnAssert;

        public Dotspec(string scenario, Func<TData> dataConstructor) : base(scenario)
        {
            if (scenario == null) throw new ArgumentNullException(nameof(scenario));
            if (dataConstructor == null) throw new ArgumentNullException(nameof(dataConstructor));

            DataConstructor = dataConstructor;
        }

        public IAdditionalPreconditionSpec<TSubject, TData> And(Action<TData> precondition)
        {
            RegisterUnitOfSpec((_, data) => precondition(data));

            return this;
        }

        public IAssertionSpec<TSubject, TData> When(Action<TSubject, TData> behaviour)
        {
            RegisterUnitOfSpec(behaviour);

            return this;
        }

        public new void Assert(Func<TData, TSubject> subjectConstructor)
        {
            var data = DataConstructor();
            var subject = subjectConstructor(data);

            RaiseOnAssert(subject, data);
        }

        public new void Assert<TException>(Func<TData, TSubject> subjectConstructor, string errorMessage) where TException : Exception
        {
            try
            {
                Assert(subjectConstructor);
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<TException>();
                ex.Message.ShouldBe(errorMessage);
            }
        }

        private void RaiseOnAssert(TSubject subject, TData data)
        {
            var callbacks = OnAssert;

            if (callbacks != null)
            {
                callbacks(subject, data);
            }
        }

        private void RegisterUnitOfSpec(Action<TSubject, TData> unitOfSpec)
        {
            OnAssert += unitOfSpec;
        }
    }
}