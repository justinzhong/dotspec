using Dotspec;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace Calculator.Behaviours
{
    /// <summary>
    /// Asserts all the facts about the behaviours of a Calculator object.
    /// </summary>
    public class CalculatorFacts
    {
        private readonly Calculator _subject;

        public CalculatorFacts()
        {
            _subject = new Calculator();
        }

        /// <summary>
        /// Specifies the theories on the behaviour of the Add() method
        /// </summary>
        private IEnumerable<Spec<Calculator>> AddMethodTheory
        {
            get
            {
                yield return "1. When adding two integers a and b, the Calculator should yield the sum of a and b.".Spec<Calculator>()
                    .Given(c => c["numberA"] = 2)
                    .Given(c => c["numberB"] = 40)
                    .Given(c => c["expectedResult"] = c.GetVal<int>("numberA") + c.GetVal<int>("numberB"))
                    .When(c => c["result"] = c.Subject.Add(c.GetVal<int>("numberA"), c.GetVal<int>("numberB")))
                    .Then(c => c.GetVal<int>("result").ShouldBe(c.GetVal<int>("expectedResult")));

                yield return "2. When adding two identical integers i, the Calculator should yield an integer twice the value of 2i.".Spec<Calculator>()
                    .Given(c => c["numberI"] = 5)
                    .Given(c => c["expectedResult"] = 2 * c.GetVal<int>("numberI"))
                    .When(c => c["result"] = c.Subject.Add(c.GetVal<int>("numberI"), c.GetVal<int>("numberI")))
                    .Then(c => c.GetVal<int>("result").ShouldBe(c.GetVal<int>("expectedResult")));
            }
        }

        /// <summary>
        /// Asserts how Add() method should work based on the specified theories.
        /// </summary>
        [Fact]
        public void How_add_method_should_work()
        {
            AddMethodTheory.Assert(_subject);
        }
    }
}
