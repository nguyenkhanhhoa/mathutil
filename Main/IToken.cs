using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShuntingYard
{
    public interface IToken { }
    public interface IOperandToken:IToken { }
    
    public class OperandToken<T> : IOperandToken
    {
        public T Value { get; }

        public OperandToken(T value)
        {
            Value = value;
        }       
    }
   
    public enum OperatorType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        OpeningBracket,
        ClosingBracket
    }

    public class OperatorToken : IToken
    {
        public OperatorType OperatorType { get; }

        public OperatorToken(OperatorType operatorType)
        {
            OperatorType = operatorType;
        }
    }
}
