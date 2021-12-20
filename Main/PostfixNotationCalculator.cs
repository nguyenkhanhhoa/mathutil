
using System;
using System.Collections.Generic;


namespace ShuntingYard
{
    public class PostfixNotationCalculator<T>
    {
        public PostfixNotationCalculator()
        {
            _operandTokensStack = new Stack<IOperandToken>();
        }

        public OperandToken<T> Calculate(IEnumerable<IToken> tokens)
        {
            Reset();
            foreach (var token in tokens)
            {
                ProcessToken(token);
            }
            return GetResult();
        }

        private void Reset()
        {
            _operandTokensStack.Clear();
        }

        private void ProcessToken(IToken token)
        {
            switch (token)
            {
                case IOperandToken/*OperandToken*/ operandToken:
                    StoreOperand(operandToken);
                    break;
                case OperatorToken operatorToken:
                    ApplyOperator(operatorToken);
                    break;
                default:
                    var exMessage = $"An unknown token type: {token.GetType()}.";
                    throw new SyntaxException(exMessage);
            }
        }

        private void StoreOperand(/*OperandToken<T>*/IOperandToken operandToken)
        {
            _operandTokensStack.Push(operandToken);
        }

        private void ApplyOperator(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                case OperatorType.Addition:
                    ApplyAdditionOperator();
                    break;
                case OperatorType.Subtraction:
                    ApplySubtractionOperator();
                    break;
                case OperatorType.Multiplication:
                    ApplyMultiplicationOperator();
                    break;
                case OperatorType.Division:
                    ApplyDivisionOperator();
                    break;
                default:
                    var exMessage = $"An unknown operator type: {operatorToken.OperatorType}.";
                    throw new SyntaxException(exMessage);
            }
        }

        private void ApplyAdditionOperator()
        {
            var operands = GetBinaryOperatorArguments();
            //var result = new OperandToken<T>(operands.Item1.Value + operands.Item2.Value);
            _operandTokensStack.Push(Add(operands.Item1.Value , operands.Item2.Value));
        }

        private void ApplySubtractionOperator()
        {
            var operands = GetBinaryOperatorArguments();
            //var result = new OperandToken<T>(operands.Item1.Value - operands.Item2.Value);
            _operandTokensStack.Push(Subtract(operands.Item1.Value , operands.Item2.Value));
        }

        private void ApplyMultiplicationOperator()
        {
            var operands = GetBinaryOperatorArguments();
            //var result = new OperandToken<T>(operands.Item1.Value * operands.Item2.Value);
            _operandTokensStack.Push(Multiply(operands.Item1.Value , operands.Item2.Value));
        }

        private void ApplyDivisionOperator()
        {
            var operands = GetBinaryOperatorArguments();
            //var result = new OperandToken<T>(operands.Item1.Value / operands.Item2.Value);
            _operandTokensStack.Push(Divide(operands.Item1.Value , operands.Item2.Value));
        }

        private Tuple<OperandToken<T>, OperandToken<T>> GetBinaryOperatorArguments()
        {
            if (_operandTokensStack.Count < 2)
            {
                var exMessage = "Not enough arguments for applying a binary operator.";
                throw new SyntaxException(exMessage);
            }

            var right = (OperandToken<T>)_operandTokensStack.Pop();
            var left = (OperandToken<T>)_operandTokensStack.Pop();

            return Tuple.Create(left, right);
        }

        private OperandToken<T> GetResult()
        {
            if (_operandTokensStack.Count == 0)
            {
                var exMessage = "The expression is invalid." +
                    " Check, please, that the expression is not empty.";
                throw new SyntaxException(exMessage);
            }
            if (_operandTokensStack.Count != 1)
            {
                var exMessage = "The expression is invalid." +
                    " Check, please, that you're providing the full expression and" +
                    " the tokens have a correct order.";
                throw new SyntaxException(exMessage);
            }

            return (OperandToken<T>)_operandTokensStack.Pop();
        }
        public static IOperandToken /*OperandToken<T> */Add(T v1, T v2)
        {
            Type type = typeof(T);
            if (type == typeof(long))
            {
                long l1 = (long)(object)v1;
                long l2 = (long)(object)v2;
                return new OperandToken<long>( l1+l2);
            }else if (type == typeof(decimal))
            {
                decimal d1 = (decimal)(object)v1;
                decimal d2 = (decimal)(object)v2;
                return new OperandToken<decimal>(d1 + d2);
            }
            else if (type == typeof(double))
            {
                double d1 = (double)(object)v1;
                double d2 = (double)(object)v2;
                return new OperandToken<double>(d1 + d2);
            }
            else if (type == typeof(Fractions.Fraction))
            {
                Fractions.Fraction f1 = (Fractions.Fraction)(object)v1;
                Fractions.Fraction f2 = (Fractions.Fraction)(object)v2;
                return new OperandToken<Fractions.Fraction>(f1 + f2);

            }
            throw new NotSupportedException($"Type {type} is not supported! ");
        }
        public static IOperandToken /*OperandToken<T> */Subtract(T v1, T v2)
        {
            Type type = typeof(T);
            if (type == typeof(long))
            {
                long l1 = (long)(object)v1;
                long l2 = (long)(object)v2;
                return new OperandToken<long>(l1 - l2);
            }
            else if (type == typeof(decimal))
            {
                decimal d1 = (decimal)(object)v1;
                decimal d2 = (decimal)(object)v2;
                return new OperandToken<decimal>(d1 - d2);
            }
            else if (type == typeof(double))
            {
                double d1 = (double)(object)v1;
                double d2 = (double)(object)v2;
                return new OperandToken<double>(d1 - d2);
            }
            else if (type == typeof(Fractions.Fraction))
            {
                Fractions.Fraction f1 = (Fractions.Fraction)(object)v1;
                Fractions.Fraction f2 = (Fractions.Fraction)(object)v2;
                return new OperandToken<Fractions.Fraction>(f1 - f2);

            }
            throw new NotSupportedException($"Type {type} is not supported! ");
        }
        public static IOperandToken /*OperandToken<T> */Multiply(T v1, T v2)
        {
            Type type = typeof(T);
            if (type == typeof(long))
            {
                long l1 = (long)(object)v1;
                long l2 = (long)(object)v2;
                return new OperandToken<long>(l1 * l2);
            }
            else if (type == typeof(decimal))
            {
                decimal d1 = (decimal)(object)v1;
                decimal d2 = (decimal)(object)v2;
                return new OperandToken<decimal>(d1 * d2);
            }
            else if (type == typeof(double))
            {
                double d1 = (double)(object)v1;
                double d2 = (double)(object)v2;
                return new OperandToken<double>(d1 * d2);
            }
            else if (type == typeof(Fractions.Fraction))
            {
                Fractions.Fraction f1 = (Fractions.Fraction)(object)v1;
                Fractions.Fraction f2 = (Fractions.Fraction)(object)v2;
                return new OperandToken<Fractions.Fraction>(f1 * f2);

            }
            throw new NotSupportedException($"Type {type} is not supported! ");
        }
        public static IOperandToken /*OperandToken<T> */Divide(T v1, T v2)
        {
            Type type = typeof(T);
            if (type == typeof(long))
            {
                long l1 = (long)(object)v1;
                long l2 = (long)(object)v2;
                return new OperandToken<double>(l1 / l2);
            }
            else if (type == typeof(decimal))
            {
                decimal d1 = (decimal)(object)v1;
                decimal d2 = (decimal)(object)v2;
                return new OperandToken<decimal>(d1 / d2);
            }
            else if (type == typeof(double))
            {
                double d1 = (double)(object)v1;
                double d2 = (double)(object)v2;
                return new OperandToken<double>(d1 / d2);
            }
            else if (type == typeof(Fractions.Fraction))
            {
                Fractions.Fraction f1 = (Fractions.Fraction)(object)v1;
                Fractions.Fraction f2 = (Fractions.Fraction)(object)v2;
                return new OperandToken<Fractions.Fraction>(f1 / f2);

            }
            throw new NotSupportedException($"Type {type} is not supported! ");
        }
        private readonly Stack<IOperandToken/*OperandToken*/> _operandTokensStack;
    }
}
