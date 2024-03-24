using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public static class Token
    {
        private static readonly string[] Operators = {"^", "*", "/", "+", "-"};

        private static int Precedence(string token)
        {
            return token switch
            {
                "*" => 3,
                "/" => 3,
                "+" => 2,
                "-" => 2,
                _ => 0
            };
        }

        public static int Evaluate(int value1, int value2, string token)
        {
            return token switch
            {
                "+" => value1 + value2,
                "-" => value1 - value2,
                "*" => value1 * value2,
                "/" => value1 / value2,
                _ => 0
            };
        }

        public static bool IsOperator(string token)
        {
            return Operators.Contains(token);
        }
        
        public static bool IsVariable(string token)
        {
            foreach (var ch in token)
            {
                if (!char.IsLetter(ch))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumber(string token)
        {
            return double.TryParse(token, out _);
        }

        public static bool IsLeftParenthesis(string token)
        {
            return token == "(";
        }

        public static bool IsRightParenthesis(string token)
        {
            return token == ")";
        }

        public static bool IsGreaterPrecedence(string token1, string token2)
        {
            return Precedence(token1) >= Precedence(token2);
        }
    }
}