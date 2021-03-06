﻿namespace Calculator
{
    /// <summary>
    /// A naive implementation of a Calculator class with a single Add() method.
    /// </summary>
    public class Calculator : ICalculator
    {
        /// <summary>
        /// Caclulates the sum of two integers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Add(int a, int b)
        {
            // Returns arithmetic overflow exception if number exceeded int.MaxValue
            return checked(a + b);
        }
    }
}
