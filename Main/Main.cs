using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShuntingYard;
using Fractions;
using Fractions.Extensions;
using System.Numerics;
using System.Collections;
namespace MathUtil
{

    public partial class Main : Form
    {
        private ShuntingYardAlgorithm<Fraction> _shuntingYardAlgorithm = new ShuntingYardAlgorithm<Fraction>();
        private PostfixNotationCalculator<Fraction> _calculator = new PostfixNotationCalculator<Fraction>();
        private Tokenizer<Fraction> _tokenizer = new Tokenizer<Fraction>();
        private ShuntingYardAlgorithm<decimal> _shuntingYardAlgorithm1 = new ShuntingYardAlgorithm<decimal>();
        private PostfixNotationCalculator<decimal> _calculator1 = new PostfixNotationCalculator<decimal>();
        private Tokenizer<decimal> _tokenizer1 = new Tokenizer<decimal>();
        public Main()
        {
            InitializeComponent();
            var a = MathExt.GreatestCommonDivisor(1515, 2727);
            var b = MathExt.LeastCommonMultiple(6, 9);
        }
        private void FractionCalulate(TextBox src, TextBox result)
        {
            try
            {
                var expression = src.Text;
                if (expression.Trim().Length == 0)
                    return;
                var infixNotationTokens = _tokenizer.Parse(expression);
                var postfixNotationTokens = _shuntingYardAlgorithm.Apply(infixNotationTokens);
                result.Text = _calculator.Calculate(postfixNotationTokens).Value.ToString();
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }
        }
        private void BtnFractionCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                FractionCalulate(this.txtFactionExpression, this.txtResult);
                FractionCalulate(this.txtFactionExpression1, this.txtResult1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.StackTrace);
            }
        }
        private void Clear(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
            }
        }
        private void BtnFractionClear_Click(object sender, EventArgs e)
        {
            this.Clear(this.tabFraction);
        }

        private void BtnDecimalClear_Click(object sender, EventArgs e)
        {
            this.Clear(this.tabDecimal);
        }
        private void DecimalCalulate(TextBox src, TextBox result)
        {
            try
            {
                var expression = src.Text.Replace(",", ".");
                if (expression.Trim().Length == 0)
                    return;
                var infixNotationTokens = _tokenizer1.Parse(expression);
                var postfixNotationTokens = _shuntingYardAlgorithm1.Apply(infixNotationTokens);
                result.Text = _calculator1.Calculate(postfixNotationTokens).Value.ToString().Replace(".", ",");
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }
        }
        private void BtnDecimalCalculate_Click(object sender, EventArgs e)
        {
            if (radio1.Checked)
            {
                DecimalCalulate(this.txtDecimalExpression, this.txtDecimalResult);
                DecimalCalulate(this.txtDecimalExpression1, this.txtDecimalResult1);
            }
            else
            {
                FractionCalulate(this.txtDecimalExpression, this.txtDecimalResult);
                FractionCalulate(this.txtDecimalExpression1, this.txtDecimalResult1);

            }
            decimal sum;
            decimal diff;
            bool error, error1;
            sum = Fractions.Extensions.MathExt.ToDecimal(this.txtDecimalSum.Text, out error);
            diff = Fractions.Extensions.MathExt.ToDecimal(this.txtDecimalDiff.Text, out error1);
            if (!error && !error1)
                this.txtDecimalResult2.Text = $"Số lớn:{(sum + diff) / 2}, Số bé: {(sum - diff) / 2}";
            decimal sum1 = Fractions.Extensions.MathExt.ToDecimal(this.txtDecimalSum1.Text, out error);
            Fraction ratio = Fractions.Extensions.MathExt.ToFraction(this.txtDecimalRatio.Text.Trim(), out error1);
            if (!error && !error1)
            {
                var onePart = sum1 / ((decimal)(ratio.Denominator + ratio.Numerator));
                this.txtDecimalResult3.Text = $"Số lớn:{onePart * ((decimal)BigInteger.Max(ratio.Denominator, ratio.Numerator))},  Số bé: {onePart * ((decimal)BigInteger.Min(ratio.Denominator, ratio.Numerator))}";
            }
            diff = Fractions.Extensions.MathExt.ToDecimal(this.txtDecimalDiff1.Text, out error);
            ratio = Fractions.Extensions.MathExt.ToFraction(this.txtDecimalRatio1.Text.Trim(), out error1);
            if (!error && !error1)
            {
                var onePart = diff / Math.Abs((decimal)(ratio.Denominator - ratio.Numerator));
                this.txtDecimalResult4.Text = $"Số lớn:{onePart * ((decimal)BigInteger.Max(ratio.Denominator, ratio.Numerator))},  Số bé: {onePart * ((decimal)BigInteger.Min(ratio.Denominator, ratio.Numerator))}";
            }
            GCD(this.txtGCD, txtDecimalResult5);
            LCM(this.txtLCM, txtDecimalResult6);
            LCD(txtLeastCommonDenominator, txtDecimalResult7);
        }
        /*private void LCD(TextBox src, TextBox result)
        {
            try
            {
                var expression = src.Text;
                if (expression.Trim().Length == 0)
                    return;
                var infixNotationTokens = _tokenizer1.Parse(expression);
                var postfixNotationTokens = _shuntingYardAlgorithm1.Apply(infixNotationTokens);
                Stack stack = new Stack();              
                foreach (var token in postfixNotationTokens)
                {
                    switch (token)
                    {
                        case IOperandToken operandToken:
                            stack.Push(token);
                            break;
                        case OperatorToken operatorToken:
                            if (operatorToken.OperatorType == OperatorType.Division)
                            {
                                if (stack.Count < 2)
                                {
                                    result.Text = "Not enough arguments for applying a binary operator.";
                                    return;
                                }
                                var right = (OperandToken<decimal>)stack.Pop();
                                var left = (OperandToken<decimal>)stack.Pop();                                                                                     
                                stack.Push(new Fraction((BigInteger)left.Value, (BigInteger)right.Value));
                            }
                            else
                            {
                                stack.Push(token);
                            }
                            break;
                        default:
                            result.Text = $"An unknown token type: {token.GetType()}.";
                            return;
                    }
                }
                var rs = "";
                Stack stack1 = new Stack();
                
                while (stack.Count > 0)
                {
                    stack1.Push(stack.Pop());
                }
                while (stack1.Count > 0)
                {
                    var left = stack1.Pop();
                    var right = stack1.Pop();
                    var op = stack1.Pop();
                }
                result.Text = _calculator.Calculate(postfixNotationTokens).Value.ToString();
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }
        }*/

        private void LCD(TextBox src, TextBox result)
        {
            try
            {
                List<String> numbers = parse(src);
                if (numbers == null)
                    return;
                List<Fraction> fractions = null;
                String rs = "";
                if (numbers.Count > 1)
                {
                    long lcm = 1;
                    fractions = new List<Fraction>();
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        bool error;
                        Fraction fraction = MathExt.ToFraction(numbers[i], out error);
                        if (!error)
                        {
                            fractions.Add(fraction);
                            System.Diagnostics.Debug.WriteLine(fraction);
                            lcm = MathExt.LeastCommonMultiple(lcm, (long)fraction.Denominator);
                        }
                    }
                    if (fractions.Count > 1)
                        foreach (var f in fractions)
                        {
                            rs += (lcm / f.Denominator * f.Numerator) + "/" + lcm + " ";
                        }
                }


                result.Text = rs;
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }
        }
        private List<long> parseNumber(TextBox src)
        {
            if (src.Text.Trim().Length == 0)
                return null;
            List<long> numbers = new List<long>();
            foreach (var num in src.Text.Trim().Split(" "))
            {
                bool error;
                long number = MathExt.ToLong(num.Trim(), out error);
                if (!error)
                    numbers.Add(number);
            }
            if (numbers.Count < 2)
            {
                numbers.Clear();
                foreach (var num in src.Text.Trim().Split(","))
                {
                    bool error;
                    long number = MathExt.ToLong(num, out error);
                    if (!error)
                        numbers.Add(number);
                }
            }
            return numbers;
        }
        private List<String> parse(TextBox src)
        {
            if (src.Text.Trim().Length == 0)
                return null;
            List<String> numbers = new List<String>();
            foreach (var num in src.Text.Trim().Split(" "))
            {
                numbers.Add(num.Trim());
            }
            if (numbers.Count < 2)
            {
                numbers.Clear();
                foreach (var num in src.Text.Trim().Split(","))
                {
                    numbers.Add(num.Trim());
                }
            }
            return numbers;
        }
        private void GCD(TextBox src, TextBox result)
        {
            try
            {
                List<long> numbers = parseNumber(src);
                if (numbers == null)
                    return;
                long gcd = 0;
                if (numbers.Capacity > 1)
                {
                    gcd = numbers[0];
                    for (int i = 1; i < numbers.Count; i++)
                    {
                        gcd = MathExt.GreatestCommonDivisor(gcd, numbers[i]);
                    }
                }
                if (gcd != 0)
                    result.Text = gcd.ToString();
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }
        }
        private void LCM(TextBox src, TextBox result)
        {
            try
            {
                List<long> numbers = parseNumber(src);
                if (numbers == null)
                    return;
                long lcm = 0;
                if (numbers.Count > 1)
                {
                    lcm = numbers[0];
                    for (int i = 1; i < numbers.Count; i++)
                    {
                        lcm = MathExt.LeastCommonMultiple(lcm, numbers[i]);
                    }
                }
                if (lcm != 0)
                    result.Text = lcm.ToString();
            }
            catch (Exception e)
            {
                result.Text = e.Message;
            }
        }
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
                this.AcceptButton = this.BtnFractionCalculate;
            else if (tabControl.SelectedIndex == 1)
                this.AcceptButton = this.BtnDecimalCalculate;
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ((CheckBox)sender).Checked;
        }
    }
}
