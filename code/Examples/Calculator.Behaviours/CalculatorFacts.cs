using Dotspec;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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
                    .Given(new
                    {
                        Number = 21,
                        ExpectedResult = (2 * 21)
                    })
                    .When(
                        (calculator, data) => calculator.Add(data.Number, data.Number))
                    .Then(
                        (data, result) => result.ShouldBe(data.ExpectedResult));

                yield return "2. When adding two random integers a and b, the Calculator should yield the sum of a and b.".Spec<Calculator>()
                    .Given(_random.Next(int.MaxValue / 2) - 1)
                    .When(
                        (calculator, number) => calculator.Add(number, number)
                    )
                    .Then(
                        (number, result) => result.ShouldBe(number * 2));

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
            AddBehaviourInlineSpecs.Last().Assert(_subject);
        }
    }
}
