using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modules.Grid
{
    public static class GridRotator<T>
    {
        public static void RotateGridClockwise(ref Grid<T> grid, int rotateCount)
        {
            for (int i = 0; i < rotateCount; i++)
            {
                RotateClockwise(ref grid);
            }
        }
        
        private static void RotateClockwise(ref Grid<T> grid)
        {
            int rows = grid.Height;
            int cols = grid.Width;
            Grid<T> rotated = new Grid<T>(new Vector2Int(rows, cols));
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[rows - 1 - i, j] = grid[j, i];
                }
            }
            grid = rotated;
        }
        
        public static void RotateGridCounterClockwise(ref Grid<T> grid, int rotateCount)
        {
            for (int i = 0; i < rotateCount; i++)
            {
                RotateCounterClockwise(ref grid);
            }
        }

        private static void RotateCounterClockwise(ref Grid<T> grid)
        {
            int rows = grid.Height;
            int cols = grid.Width;
            Grid<T> rotated = new Grid<T>(new Vector2Int(rows, cols));
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[i, cols - 1 - j] = grid[j, i];
                }
            }
            grid = rotated;
        }
    }
}