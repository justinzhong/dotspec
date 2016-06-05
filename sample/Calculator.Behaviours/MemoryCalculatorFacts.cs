using Dotspec;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Calculator.Behaviours
{
    public class MemoryCalculatorFacts
    {
        private static readonly Random _random = new Random();
        private readonly ICalculator _calculator;

        public MemoryCalculator Subject
        {
            get
            {
                return new MemoryCalculator(_calculator);
            }
        }

        public MemoryCalculatorFacts()
        {
            _calculator = Substitute.For<ICalculator>();
        }

        public IEnumerable<Spec<MemoryCalculator>> AddBehaviours
        {
            get
            {
                yield return "1. When adding two numbers, the MemoryCalculator should return the expected result and retain the result in memory.".Spec<MemoryCalculator>()
                    .Given(
                        _random.Next(int.MaxValue) - 1, // Given a random number
                        number => new                   // Create a slightly more complex test data model
                        {
                            Number = number,
                            Result = number * 2
                        })
                    .And(
                        // Mock the dependency calculator object to return expected result
                        data => _calculator.Add(data.Number, data.Number).Returns(data.Result))
                    .When(
                        // Run the behaviour under test
                        (calculator, data) => calculator.Add(data.Number, data.Number))
                    .Then(
                        // Assert result is as expected
                        (data, result) => result.ShouldBe(data.Result))
                    .And(
                        // Assert calculator retained the result in memory
                        (calculator, data) => calculator.Result.ShouldBe(data.Result));
            }
        }

        [Fact]
        public void AssertAddBehaviour()
        {
            AddBehaviours.Assert(Subject);
        }
    }
}
