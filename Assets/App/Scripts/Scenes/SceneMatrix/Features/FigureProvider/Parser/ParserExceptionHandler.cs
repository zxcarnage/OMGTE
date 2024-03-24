using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public static class ParserExceptionHandler
    {
        private const string IncorrectSizeMessage = "Incorrect width/height format!";
        private const string IncorrectLinesCountMessage = "Incorrect file lines count!";
        private const string IncorrectFigureStringFormatMessage = "Incorrect figure string format!";
        
        public static void CheckIndexesException(List<string> stringIndexes, out List<int> figureIndexes, int width, int height)
        {
            try
            {
                figureIndexes = stringIndexes.Select(int.Parse).ToList();
                if (figureIndexes.Any(x => x >= (width * height)))
                    throw new Exception();
            }
            catch (Exception)
            {
                throw new ExceptionParseFigure(IncorrectFigureStringFormatMessage);
            }
        }

        public static void CheckFile(string[] fileLines)
        {
            if (fileLines.Length != 3)
                throw new ExceptionParseFigure(IncorrectLinesCountMessage);
        }

        public static void CheckSizeException(out int width, out int height, string[] fileLines)
        {
            try
            {
                width = int.Parse(fileLines[0]);
                height = int.Parse(fileLines[1]);
            }
            catch (Exception)
            {
                throw new ExceptionParseFigure(IncorrectSizeMessage);
            }
        }
    }
}