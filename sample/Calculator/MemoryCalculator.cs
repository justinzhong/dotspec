using System;

namespace Calculator
{
    public class MemoryCalculator : ICalculator
    {
        public int Result { get; private set; }

        private readonly ICalculator _calculator;

        public MemoryCalculator(ICalculator calculator)
        {
            if (calculator == null) throw new ArgumentNullException("calculator");

            _calculator = calculator;
        }

        public int Add(int a, int b)
        {
            Result = _calculator.Add(a, b);

            return Result;
        }
    }
}
