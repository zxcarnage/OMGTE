using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public class ParserFigureDummy : IFigureParser
    {
        private Grid<bool> _grid;
        public Grid<bool> ParseFile(string text)
        {
            try
            {
                string[] fileLines = text.Split(Environment.NewLine);
                ParserExceptionHandler.CheckSizeException(out int width, out int height, fileLines);
                _grid = new Grid<bool>(new Vector2Int(width, height));
                List<string> figureStringIndexes = fileLines[2].Split(' ').ToList();
                figureStringIndexes.RemoveAll(s => string.IsNullOrWhiteSpace(s));
                ParserExceptionHandler.CheckIndexesException(figureStringIndexes, out List<int> figureIndexes, width, height);
                ParserExceptionHandler.CheckFile(fileLines);
                FullfillGrid(figureIndexes);
                if (!IsFigureConnected())
                    throw new ExceptionParseFigure("Figure cannot be tetris-like, because it isn't connected!");
                return _grid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        
        private void FullfillGrid(List<int> indexes)
        {
            int x;
            int y;
            foreach (var index in indexes)
            {
                x = index % _grid.Width;
                y = _grid.Height - 1 - index / _grid.Width;
                _grid[x, y] = true;
            }
        }

        private bool IsFigureConnected()
        {
            FindFirstFigureIndex(out int x, out int y, _grid);
            var isConnected = BFSCheck(x, y);
            return isConnected;
        }

        private bool FindFirstFigureIndex(out int x, out int y, Grid<bool> grid)
        {
            x = 0;
            y = 0;
            for (var i = 0; i < grid.Height; i++)
            {
                for (var j = 0; j < grid.Width ; j++)
                {
                    if (grid[j, i])
                    {
                        x = i;
                        y = j;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool BFSCheck(int startX, int startY)
        {
            CopyGrid(out Grid<bool> gridCopy);
            BFS(startX, startY, gridCopy);
            if (FindFirstFigureIndex(out int x, out int y, gridCopy))
                return false;
            return true;
        }

        private void BFS(int startX, int startY, Grid<bool> grid)
        {
            int rows = _grid.Width;
            int cols = _grid.Height;
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            Queue<(int,int)> queue = new Queue<(int,int)>();
            queue.Enqueue((startX, startY));
            grid[startX, startY] = false;

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                for (var i = 0; i < 4; i++)
                {
                    int nx = x + dx[i];
                    int ny = y + dy[i];
                    if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && grid[nx, ny])
                    {
                        queue.Enqueue((nx, ny));
                        grid[nx, ny] = false;
                    }
                }
            }
        }

        private void CopyGrid(out Grid<bool> to)
        {
            to = new Grid<bool>(new Vector2Int(_grid.Width, _grid.Height));
            for (var i = 0; i < _grid.Width; i++)
            {
                for (var j = 0; j < _grid.Height; j++)
                {
                    to[i, j] = _grid[i, j];
                }
            }
        }
    }
}