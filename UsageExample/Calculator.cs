using System;
using AdvancedConsole.Commands.Attributes;

namespace UsageExample
{
    [Module]
    public class Calculator
    {
        private double _temp = 0;

        public double Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                Console.WriteLine(value);
            }
        }

        [Command]
        public void Clear() => Temp = 0;
        [Command]
        public void Add(double n) => Temp += n;
        [Command]
        public void Subtract(double n) => Temp -= n;
        [Command]
        public void Multiply(double n) => Temp *= n;
        [Command]
        public void Divide(double n) => Temp /= n;
        [Command]
        public void Pow(double n) => Temp = Math.Pow(Temp, n);
        [Command]
        public void Root(double n) => Temp = Math.Pow(Temp, 1/n);
    }
}