﻿using System;

namespace Dotspec
{
    /// <summary>
    /// Records the preconditions and input for a test specification.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public class PreconditionSpec<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Sole constructor.
        /// </summary>
        /// <param name="scenario"></param>
        public PreconditionSpec(string scenario) : base(scenario) { }

        /// <summary>
        /// Transitions to PreconditionSpec object which accepts TData as one of
        /// its type definitions.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject, TData> Given<TData>(TData data)
        {
            return SpecFactory.BuildPreconditionAliasWithDataSpec(Scenario, data, OnAssert);
        }

        public PreconditionSpec<TSubject, TData2> Given<TData, TData2>(TData data, Func<TData, TData2> dataTemplate)
        {
            return SpecFactory.BuildPreconditionAliasWithDataSpec(Scenario, dataTemplate(data), OnAssert);
        }

        public PreconditionSpec<TSubject> Given(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition());

            return this;
        }

        /// <summary>
        /// Records a precondition for this test specification.
        /// </summary>
        /// <param name="precondition"></param>
        /// <returns></returns>
        public PreconditionSpec<TSubject> Given(Action<TSubject> precondition)
        {
            if (precondition == null) throw new ArgumentNullException("precondition");

            RegisterAssertionCallback((sender, subject) => precondition(subject));

            return this;
        }

        /// <summary>
        /// Transitions to AssertionSpec which captures the specified behaviour.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public AssertionSpec<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour)
        {
            return SpecFactory.BuildAssertionSpec(Scenario, behaviour, OnAssert);
        }
    }
}
