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
using System.Numerics;

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
        }
        private void FractionCalulate(TextBox src, TextBox result)
        {
            var expression = src.Text;
            if (expression.Trim().Length == 0)
                return;           
            var infixNotationTokens = _tokenizer.Parse(expression);
            var postfixNotationTokens = _shuntingYardAlgorithm.Apply(infixNotationTokens);
            result.Text = _calculator.Calculate(postfixNotationTokens).Value.ToString();
            
        }
        private void BtnFractionCalculate_Click(object sender, EventArgs e)
        {           
            try
            {
                FractionCalulate(this.txtFactionExpression, this.txtResult);
                FractionCalulate(this.txtFactionExpression1, this.txtResult1);
            }
            catch(Exception ex)
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
            var expression = src.Text.Replace(",",".");
            if (expression.Trim().Length == 0)
                return;
            var infixNotationTokens = _tokenizer1.Parse(expression);
            var postfixNotationTokens = _shuntingYardAlgorithm1.Apply(infixNotationTokens);
            result.Text = _calculator1.Calculate(postfixNotationTokens).Value.ToString().Replace(".",",");

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
            bool error,error1;
            sum = Fractions.Extensions.MathExt.toDecimal(this.txtDecimalSum.Text, out error);
            diff = Fractions.Extensions.MathExt.toDecimal(this.txtDecimalDiff.Text, out error1);
            if(!error && !error1)
                this.txtDecimalResult2.Text = $"Số lớn:{(sum+diff)/2}, Số bé: {(sum - diff) / 2}";
            decimal sum1 = Fractions.Extensions.MathExt.toDecimal(this.txtDecimalSum1.Text,out error);
            Fraction ratio = Fractions.Extensions.MathExt.toFraction(this.txtDecimalRatio.Text.Trim(),out error1);
            if (!error && !error1)
            {                
                var onePart = sum1 / ((decimal)(ratio.Denominator + ratio.Numerator));                             
                this.txtDecimalResult3.Text = $"Số lớn:{onePart* ((decimal)BigInteger.Max(ratio.Denominator, ratio.Numerator))},  Số bé: {onePart* ((decimal)BigInteger.Min(ratio.Denominator, ratio.Numerator))}";
            }
            diff = Fractions.Extensions.MathExt.toDecimal(this.txtDecimalDiff1.Text, out error);
            ratio = Fractions.Extensions.MathExt.toFraction(this.txtDecimalRatio1.Text.Trim(), out error1);
            if (!error && !error1)
            {
                var onePart = diff / Math.Abs((decimal)(ratio.Denominator - ratio.Numerator));
                this.txtDecimalResult4.Text = $"Số lớn:{onePart * ((decimal)BigInteger.Max(ratio.Denominator, ratio.Numerator))},  Số bé: {onePart * ((decimal)BigInteger.Min(ratio.Denominator, ratio.Numerator))}";
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
