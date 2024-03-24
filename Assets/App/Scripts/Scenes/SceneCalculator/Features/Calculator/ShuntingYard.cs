using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public static class ShuntingYard
    {
        public static Queue<string> GetPostfixQueue(string input)
        {
            var outputQueue = new Queue<string>();
            var operandStack = new Stack<string>();
            
            var regex = new Regex(@"(?:[+-]?\d*\.)?\d+|[a-zA-Z]+|[-+*/()]");
            var inputTokens = regex.Matches(input).Select(m => m.Value).ToList();

            if (inputTokens.Count >= 2 && inputTokens[0] == "-" && int.TryParse(inputTokens[1], out var num))
            {
                inputTokens[1] = string.Concat(inputTokens[0], inputTokens[1]);
                inputTokens.Remove(inputTokens[0]);
            }

            foreach (var token in inputTokens)
            {
                if (Token.IsNumber(token) || Token.IsVariable(token))
                {
                    outputQueue.Enqueue(token);
                    continue;
                }

                if (Token.IsLeftParenthesis(token))
                {
                    operandStack.Push(token);
                    continue;
                }

                if (Token.IsRightParenthesis(token))
                {
                    while (!Token.IsLeftParenthesis(operandStack.Peek()))
                    {
                        outputQueue.Enqueue(operandStack.Pop());
                    }

                    operandStack.Pop();
                    continue;
                }

                while (operandStack.Any() && Token.IsGreaterPrecedence(operandStack.Peek(), token))
                {
                    outputQueue.Enqueue(operandStack.Pop());
                }

                operandStack.Push(token);
            }

            while (operandStack.Count > 0)
                outputQueue.Enqueue(operandStack.Pop());

            return outputQueue;
        }
    }
}