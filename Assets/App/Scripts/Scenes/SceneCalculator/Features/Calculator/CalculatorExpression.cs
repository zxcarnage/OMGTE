using System.Collections.Generic;
using System.Linq;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {
        private readonly Dictionary<string, string> _expressions;

        public CalculatorExpression()
        {
            _expressions = new Dictionary<string, string>();
        }
        
        public int Execute(string expression)
        {
            var resultStack = new Stack<int>();
            var postfixQueue = ShuntingYard.GetPostfixQueue(expression);

            while (postfixQueue.Any())
            {
                var currentToken = postfixQueue.Dequeue();

                if (Token.IsNumber(currentToken))
                {
                    resultStack.Push(int.Parse(currentToken));
                    continue;
                }
                
                if (Token.IsVariable(currentToken))
                {
                    resultStack.Push(Get(currentToken));
                    continue;
                }

                if (Token.IsOperator(currentToken))
                {
                    var val1 = resultStack.Pop();
                    var val2 = resultStack.Pop();
                    var output = Token.Evaluate(val2, val1, currentToken);
                    resultStack.Push(output);
                }
            }

            return resultStack.Pop();
        }

        public void SetExpression(string expressionKey, string expression)
        {
            CalculatorExceptionHandler.CheckExpression(expression, expressionKey);
            CalculatorExceptionHandler.CheckRepeatDefineKey(_expressions,expressionKey);
            _expressions[expressionKey] = expression;
        }

        public int Get(string expressionKey)
        {
            CalculatorExceptionHandler.CheckDefinedKey(_expressions, expressionKey);
            return Execute(_expressions[expressionKey]);
        }
    }
}