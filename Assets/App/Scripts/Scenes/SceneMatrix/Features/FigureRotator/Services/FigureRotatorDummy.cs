using App.Scripts.Modules.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services
{
    public class FigureRotatorDummy : IFigureRotator
    {
        public Grid<bool> RotateFigure(Grid<bool> grid, int rotateCount)
        {
            rotateCount %= 4;
            switch (rotateCount)
            {
                case 0:
                    return grid;
                case > 0:
                    GridRotator<bool>.RotateGridClockwise(ref grid, rotateCount);
                    break;
                default:
                    GridRotator<bool>.RotateGridCounterClockwise(ref grid,Mathf.Abs(rotateCount));
                    break;
            }

            return grid;
        }
    }
}