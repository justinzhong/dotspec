using System;

namespace Dotspec
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public abstract class AssertionSpecBase<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        public AssertionSpecBase(string scenario) : base(scenario) { }

        public ExceptionSpec<TSubject, TException> Throws<TException>(string exceptionMessage)
            where TException : Exception
        {
            return SpecFactory.BuildExceptionSpec<TException>(Scenario, exceptionMessage, OnAssert);
        }
    }
}
