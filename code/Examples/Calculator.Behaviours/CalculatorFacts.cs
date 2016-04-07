using Dotspec;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Calculator.Behaviours
{
    /// <summary>
    /// Asserts all the behaviours of a Calculator object match with 
    /// specifications.
    /// </summary>
    public class CalculatorFacts
    {
        private static readonly Random _random = new Random();

        private readonly Calculator _subject;

        public CalculatorFacts()
        {
            _subject = new Calculator();
        }

        /// <summary>
        /// Defines the test specifications for the Calculator's add behaviour.
        /// </summary>
        private IEnumerable<Spec<Calculator>> AddBehaviourInlineSpecs
        {
            get
            {
                yield return "1. When adding two identical integers a, the result should be (2 x a).".Spec<Calculator>()
                    .Given(_random.Next(int.MaxValue / 2) - 1)
                    .When(
                        (calculator, number) => calculator.Add(number, number)
                    )
                    .Then(
                        (number, result) => result.ShouldBe(2 * number));

                yield return "2. When adding two random integers x and y, the Calculator should yield the sum of x and y.".Spec<Calculator>()
                    .Given(new {
                        X = _random.Next(int.MaxValue / 2) - 1,
                        Y = _random.Next(int.MaxValue / 2) - 1})
                    .When(
                        (calculator, number) => calculator.Add(number.X, number.Y)
                    )
                    .Then(
                        (number, result) => result.ShouldBe(number.X + number.Y));

                yield return "3. When adding two maximum integers, the Calculator should throw an arithmetic overflow exception.".Spec<Calculator>()
                    .Given(int.MaxValue)
                    .When((calculator, number) => calculator.Add(number, number))
                    .Throws<OverflowException>();
            }
        }

        /// <summary>
        /// Asserts the Calculator's add behaviour.
        /// </summary>
        [Fact]
        public void AssertAddBehaviourWithInlineSpec()
        {
            AddBehaviourInlineSpecs.Assert(_subject);
        }
    }
}
