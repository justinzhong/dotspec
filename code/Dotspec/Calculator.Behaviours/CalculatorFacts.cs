using Dotspec;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Calculator.Behaviours
{
    /// <summary>
    /// Asserts all the facts about the behaviours of a Calculator object.
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
        /// Specifies the theories on the Calculator's add behaviour.
        /// </summary>
        private IEnumerable<Spec<Calculator>> AddBehaviourInlineSpecs
        {
            get
            {
                yield return "1. When adding two identical integers a, the result should be (2 x a).".Spec<Calculator>()
                    .Given(new {
                        Number = 21,
                        ExpectedResult = (2 * 21)
                    })
                    .When(
                        (calculator, data) => calculator.Add(data.Number, data.Number))
                    .Then(
                        (data, result) => result.ShouldBe(data.ExpectedResult));

                yield return "2. When adding two random integers a and b, the Calculator should yield the sum of a and b.".Spec<Calculator>()
                    .Given(new {
                        A = _random.Next(int.MaxValue / 2),
                        B = _random.Next(int.MaxValue / 2)
                    })
                    .When(
                        (calculator, data) => calculator.Add(data.A, data.B)
                    )
                    .Then(
                        (data, result) => result.ShouldBe(data.A + data.B));
            }
        }

        ///// <summary>
        ///// Specifies the theories on the Calculator's add behaviour.
        ///// </summary>
        //private IEnumerable<Spec<Calculator>> AddBehaviourSpecData
        //{
        //    get
        //    {
        //        yield return "1. When adding two identical integers a, the result should be (2 x a).".Spec<Calculator>()
        //            .Given(new int[] { 21, 21, (2 * 21) });

        //        var a = _random.Next(int.MaxValue / 2);
        //        var b = _random.Next(int.MaxValue / 2);

        //        yield return "2. When adding two random integers a and b, the result should yield the sum of a and b.".Spec<Calculator>()
        //            .Given(new int[] { a, b, (a + b) });
        //    }
        //}

        /// <summary>
        /// Asserts the Calculator's add behaviour.
        /// </summary>
        [Fact]
        public void AssertAddBehaviourWithInlineSpec()
        {
            AddBehaviourInlineSpecs.Assert(_subject);
        }

        //public void AssertAddBehaviourWithSpecData()
        //{
        //    var addBehaviourSpec = "Add behaviour spec".Spec<Calculator>()
        //        .Given(new int[] { })
        //        .When(
        //            (calculator, data) => calculator.Add(data[0], data[1]))
        //        .Then((data, result) => result.ShouldBe(data[2]));

        //    //addBehaviourSpec.Assert(_subject, AddBehaviourSpecData);
        //}
    }
}
