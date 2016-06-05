using System;
using Shouldly;

namespace Dotspec
{
    public class Dotspec<TSubject> : IPreconditionSpec<TSubject>, IBehaviourJunctionSpec<TSubject>, IAssertionSpec<TSubject>
        where TSubject : class
    {
        private string Scenario { get; }

        private event Action<TSubject> OnAssert;

        public Dotspec(string scenario)
        {
            Scenario = scenario;
        }

        public IBehaviourJunctionSpec<TSubject> Given(Action precondition)
        {
            RegisterUnitOfSpec(_ => precondition());

            return this;
        }

        public IBehaviourJunctionSpec<TSubject> Given(Action<TSubject> precondition)
        {
            RegisterUnitOfSpec(precondition);

            return this;
        }

        public IBehaviourJunctionSpec<TSubject> And(Action precondition)
        {
            RegisterUnitOfSpec(_ => precondition());

            return this;
        }

        public IBehaviourJunctionSpec<TSubject> And(Action<TSubject> precondition)
        {
            RegisterUnitOfSpec(precondition);

            return this;
        }

        public IAssertionSpec<TSubject> When(Action<TSubject> behaviour)
        {
            RegisterUnitOfSpec(behaviour);

            return this;
        }

        public void Assert(TSubject subject)
        {
            RaiseOnAssert(subject);
        }

        public void Assert<TException>(TSubject subject, string errorMessage)
            where TException : Exception
        {
            try
            {
                RaiseOnAssert(subject);
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<TException>();
                ex.Message.ShouldBe(errorMessage);
            }
        }

        private void RegisterUnitOfSpec(Action<TSubject> unitOfSpec)
        {
            OnAssert += unitOfSpec;
        }

        private void RaiseOnAssert(TSubject subject)
        {
            var callbacks = OnAssert;

            if (callbacks != null)
            {
                callbacks(subject);
            }
        }
    }
}
