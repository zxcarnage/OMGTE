using System.Collections.Generic;
using System.Linq;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public static class CalculatorExceptionHandler
    {
        private const string SelfContainExceptionMessage = "Expression cannot contain variable on itself!";
        private const string ExpressionAlreadyDefinedMessage = "Expression was already defined!";
        private const string ExpressionNotDefinedMessage = "Expression wasn't defined!";
        public static void CheckExpression(string expression, string expressionKey)
        {
            if (expression.Contains(expressionKey))
            {
                throw new ExceptionExecuteExpression(SelfContainExceptionMessage);
            }
        }

        public static void CheckRepeatDefineKey(Dictionary<string,string> expressions, string expressionKey)
        {
            if (CheckExpressionKey(expressions, expressionKey))
                throw new ExceptionExecuteExpression(ExpressionAlreadyDefinedMessage);
        }

        public static void CheckDefinedKey(Dictionary<string, string> expressions, string expressionKey)
        {
            if (!CheckExpressionKey(expressions, expressionKey))
                throw new ExceptionExecuteExpression(ExpressionNotDefinedMessage);
        }

        private static bool CheckExpressionKey(Dictionary<string,string> expressions, string expressionKey)
        {
            return expressions.ContainsKey(expressionKey);
        }
    }
}