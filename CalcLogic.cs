using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsCalculator
{
    interface Operation
    {
        public Number Compute(Number number1, Number number2);
        public string Display { get; }
    }
    interface UnaryOperation
    {
        public Number Compute(Number number);
    }
    class OperPlus : Operation
    {
        public Number Compute(Number number1, Number number2)
        {
            var result = new Number();
            result.Normal = number1.Normal + number2.Normal;
            return result;
        }
        public string Display { get; } = "+";
    }
    class OperMinus : Operation
    {
        public Number Compute(Number number1, Number number2)
        {
            var result = new Number();
            result.Normal = number1.Normal - number2.Normal;
            return result;
        }
        public string Display { get; } = "-";
    }
    class OperMultiply : Operation
    {
        public Number Compute(Number number1, Number number2)
        {
            var result = new Number();
            result.Normal = number1.Normal * number2.Normal;
            return result;
        }
        public string Display { get; } = "*";
    }
    class OperDivide : Operation
    {
        public Number Compute(Number number1, Number number2)
        {
            var result = new Number();
            result.Normal = number1.Normal / number2.Normal;
            return result;
        }
        public string Display { get; } = "/";
    }
    class OperTakePercent : Operation
    {
        public Number Compute(Number number1, Number number2)
        {
            var result = new Number();
            result.Normal = number2.Normal / 100 * number1.Normal;
            return result;
        }
        public string Display { get; } = "%";
    }

    class OperSignInvert : UnaryOperation
    {
        public Number Compute(Number number)
        {
            var result = new Number();
            result.Normal = -number.Normal;
            return result;
        }
    }
    class OperSQRT : UnaryOperation
    {
        public Number Compute(Number number)
        {
            var result = new Number();
            try
            {
                result.Normal = (decimal)Math.Sqrt((double)number.Normal);
                return result;
            }
            catch { return result; }
        }
    }
    class OperOneByX : UnaryOperation
    {
        public Number Compute(Number number)
        {
            var result = new Number();
            result.Normal = (decimal)((decimal)1 / number.Normal);
            return result;
        }
    }
}

public class Number
{
    private decimal normal = 0;
    private int ptIndex = 0;

    public Number()
    {
    }
    public Number(int intPart, int decimalPart)
    {
    }
    public decimal Normal
    {
        get{return normal;}
        set{normal = value;}
    }
    public int PointIndex
    {
        get { return ptIndex; }
        set { ptIndex = value; }
    }
    public void AddDigitToEnd(int digit)
    {
        if (digit >= 0 && digit <= 9)
        {
            if (ptIndex != 0)
            {
                if (ptIndex >= 10) return;
                normal = normal + (decimal)digit / ((int)Math.Pow(10, ptIndex));
                ptIndex++;
            }
            else
            {
                if (Math.Abs(normal) * 10 + digit < decimal.MaxValue)
                {
                    decimal prev = normal;
                    normal = Math.Abs(normal) * 10 + digit;
                    if (prev < 0) normal = -normal;
                }
            }
        }
    }
    public void RemoveDigitFromEnd()
    {
        if (ptIndex > 2)
        {
            int power = (int)Math.Pow(10, ptIndex - 2);
            normal = Math.Floor(normal * power) / (power);
            if (ptIndex == 3) ptIndex = 0;
            else ptIndex--;
        }
        else
        {
            normal = Math.Floor(normal / 10);
        }
    }
    public void AppendPoint()
    {
        PointIndex = 1;
    }
}