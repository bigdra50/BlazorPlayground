using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable enable
namespace BlazorTodo.Pages
{
    interface IMsg
    {
        public CalculatorModel Operate(CalculatorModel currentModel);
    }

    class PushDigit : IMsg
    {
        private readonly char _value;

        public PushDigit(char c)
        {
            _value = c;
        }

        public CalculatorModel Operate(CalculatorModel model) =>
            new CalculatorModel(model.InfixExpression == "0" ? _value.ToString() : model.InfixExpression + _value, model);
    }

    class Clear : IMsg
    {
        public CalculatorModel Operate(CalculatorModel model)
        {
            if (model.InfixExpression == "0") return model;
            return new CalculatorModel(model.InfixExpression.Substring(0, model.InfixExpression.Length - 1), model);
        }
    }

    class AllClear : IMsg
    {
        public CalculatorModel Operate(CalculatorModel model)
        {
            return new CalculatorModel();
        }
    }

    #region Operator

    class Plus : IMsg
    {
        private const char Identifier = '+';

        public CalculatorModel Operate(CalculatorModel model)
        {
            if (model.InfixExpression == "") return model;
            if (Regex.IsMatch(model.InfixExpression, @"[+-/*]$"))
            {
                var clearedModel = new Clear().Operate(model);
                Console.WriteLine("Match");
                return new CalculatorModel(clearedModel.InfixExpression + Identifier, clearedModel);
            }

            var infix = model.InfixExpression + Identifier;
            return new CalculatorModel(infix, model);
        }
    }

    class Minus : IMsg
    {
        private const char Identifier = '-';

        public CalculatorModel Operate(CalculatorModel model)
        {
            if (model.InfixExpression == "") return model;
            if (Regex.IsMatch(model.InfixExpression, @"[+-/*]$"))
            {
                var clearedModel = new Clear().Operate(model);
                Console.WriteLine("Match");
                return new CalculatorModel(clearedModel.InfixExpression + Identifier, clearedModel);
            }

            var infix = model.InfixExpression + Identifier;
            return new CalculatorModel(infix, model);
        }
    }

    class Multiply : IMsg
    {
        private const char Identifier = '*';

        public CalculatorModel Operate(CalculatorModel model)
        {
            if (model.InfixExpression == "") return model;
            if (Regex.IsMatch(model.InfixExpression, @"[+-/*]$"))
            {
                var clearedModel = new Clear().Operate(model);
                return new CalculatorModel(clearedModel.InfixExpression + Identifier, clearedModel);
            }

            var infix = model.InfixExpression + Identifier;
            return new CalculatorModel(infix, model);
        }
    }

    class Division : IMsg
    {
        private const char Identifier = '/';

        public CalculatorModel Operate(CalculatorModel model)
        {
            if (model.InfixExpression == "") return model;
            if (Regex.IsMatch(model.InfixExpression, @"[+-/*]$"))
            {
                var clearedModel = new Clear().Operate(model);
                return new CalculatorModel(clearedModel.InfixExpression + Identifier, clearedModel);
            }

            var infix = model.InfixExpression + Identifier;
            return new CalculatorModel(infix, model);
        }
    }

    class Equal : IMsg
    {
        public CalculatorModel Operate(CalculatorModel model)
        {
            if (model.InfixExpression == "") return model;
            var reversePolish = model.ReversePolishNotation();
            model.Calculate(reversePolish);
            return new CalculatorModel(model.Answer.ToString(), model);
        }
    }

    #endregion

    public class CalculatorModel
    {
        public double Answer => _answer;

        public string InfixExpression => _infixExpression;

        private double _answer;
        private string _infixExpression;

        #region ctor

        public CalculatorModel()
        {
            _answer = 0;
            _infixExpression = "0";
        }

        public CalculatorModel(CalculatorModel model)
        {
            _infixExpression = model._infixExpression;
            _answer = model.Answer;
        }

        public CalculatorModel(string infixExpression, CalculatorModel model)
        {
            _infixExpression = infixExpression;
            _answer = model.Answer;
        }

        public CalculatorModel(string infixExpression, double answer, CalculatorModel model)
        {
            _answer = answer;
            _infixExpression = infixExpression;
        }

        #endregion

        // TODO: 複雑度がやばたにえん
        public List<string> ReversePolishNotation()
        {
            var digits = new List<string>();
            var operatorStack = new Stack<char>();
            var digitsQueue = new Queue<char>();
            foreach (var item in _infixExpression)
            {
                digitsQueue.Enqueue(item);
            }

            while (digitsQueue.Count != 0)
            {
                var digit = "";
                // 連続した数字を1つの項にしたい
                while (digitsQueue.TryPeek(out var maybeDigit))
                {
                    if (!char.IsDigit(maybeDigit)) break;
                    digit += digitsQueue.Dequeue();
                }

                if (digit != "")
                {
                    digits.Add(digit);
                }

                if (digitsQueue.TryPeek(out var operatorChar))
                {
                    // 演算子の優先度チェック
                    //TODO: クソ深ネスト. 演算子に優先度とか定義して
                    if (operatorStack.TryPeek(out var o))
                    {
                        if (o == '*' || o == '/')
                        {
                            if (operatorChar == '+' || operatorChar == '-')
                            {
                                digits.Add(operatorStack.Pop().ToString());
                            }
                            else if (operatorChar == '*' || operatorChar == '/')
                            {
                                operatorStack.Push(digitsQueue.Dequeue());
                            }
                        }
                        else if (o == '+' || o == '-')
                        {
                            if (operatorChar == '*' || operatorChar == '/')
                            {
                                operatorStack.Push(digitsQueue.Dequeue());
                            }
                            else if (operatorChar == '+' || operatorChar == '-')
                            {
                                operatorStack.Push(digitsQueue.Dequeue());
                            }
                        }
                    }
                    else
                    {
                        operatorStack.Push(digitsQueue.Dequeue());
                    }
                }
            }

            while (operatorStack.Count != 0)
            {
                if (operatorStack.TryPop(out var o))
                {
                    digits.Add(o.ToString());
                }
            }


            return digits;
        }

        public void Calculate(List<string> reversePolish)
        {
            var valueStack = new Stack<double>();
            foreach (var r in reversePolish)
            {
                if (double.TryParse(r, out var value))
                {
                    valueStack.Push(value);
                }
                else
                {
                    if (!valueStack.TryPop(out var y) || !valueStack.TryPop(out var x)) continue;
                    var result = r switch
                    {
                        "+" => x + y,
                        "-" => x - y,
                        "*" => x * y,
                        "/" => Math.Abs(y) >= .0000000001 ? x / y : 0,
                        _ => throw new InvalidOperationException()
                    };
                    valueStack.Push(result);
                }
            }

            _answer = valueStack.Pop();
        }
    }
}