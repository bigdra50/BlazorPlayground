using System.Collections.Generic;
using BlazorTodo.Pages;
using NUnit.Framework;

namespace BlazorPlayGround.UnitTests
{
    [TestFixture]
    public class CalculatorModelTests
    {
        [Test]
        public void ReversePolishNotation_Addition1Digits()
        {
            var model = new CalculatorModel("1+2+3", new CalculatorModel());
            var expect = new List<string>
            {
                "1", "2", "3", "+", "+"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_AdditionSomeDigits()
        {
            var model = new CalculatorModel("10+122+2343", new CalculatorModel());
            var expect = new List<string>
            {
                "10", "122", "2343", "+", "+"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_Minus1Digits()
        {
            var model = new CalculatorModel("1-2-3", new CalculatorModel());
            var expect = new List<string>
            {
                "1", "2", "3", "-", "-"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_MinusSomeDigits()
        {
            var model = new CalculatorModel("10-122-2343", new CalculatorModel());
            var expect = new List<string>
            {
                "10", "122", "2343", "-", "-"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_Multiple1Digits()
        {
            var model = new CalculatorModel("1*2*3", new CalculatorModel());
            var expect = new List<string>
            {
                "1", "2", "3", "*", "*"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_MultipleSomeDigits()
        {
            var model = new CalculatorModel("10*122*2343", new CalculatorModel());
            var expect = new List<string>
            {
                "10", "122", "2343", "*", "*"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_Divide1Digits()
        {
            var model = new CalculatorModel("1/2/3", new CalculatorModel());
            var expect = new List<string>
            {
                "1", "2", "3", "/", "/"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_DivideSomeDigits()
        {
            var model = new CalculatorModel("10/122/2343", new CalculatorModel());
            var expect = new List<string>
            {
                "10", "122", "2343", "/", "/"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_Mix1Digits()
        {
            var model = new CalculatorModel("1+2*3-4+5", new CalculatorModel());
            var expect = new List<string>
            {
                "1", "2", "3", "*", "4", "5", "+", "-", "+"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }

        [Test]
        public void ReversePolishNotation_MixSomeDigits()
        {
            var model = new CalculatorModel("10+20*30-40+50", new CalculatorModel());
            var expect = new List<string>
            {
                "10", "20", "30", "*", "40", "50", "+", "-", "+"
            };
            Assert.That(model.ReversePolishNotation(), Is.EqualTo(expect));
        }


        [Test]
        public void Calculate_Addition1()
        {
            var model = new CalculatorModel("1+2+3", new CalculatorModel());
            var reversePolish = new List<string>
            {
                "1", "2", "3", "+", "+"
            };
            model.Calculate(reversePolish);
            var expect = 6d;
            Assert.AreEqual(model.Answer, expect);
        }
    }
}