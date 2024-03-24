using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding.Factory
{
    public class NeighbourPathFactory : INeighbourPathFactory
    {
        public List<Vector2Int> ReceiveNeighbours(UnitType unitType, Vector2Int unitCell, Grid<int> gridMatrix)
        {
            switch (unitType)
            {
                case UnitType.SwordMan:
                    return GetSwordManPath(gridMatrix, unitCell);
                case UnitType.HorseMan:
                    return GetHorseManPath(gridMatrix, unitCell);
                case UnitType.Angel:
                    return GetAngelPath(gridMatrix, unitCell);
                case UnitType.Barbarian:
                    return GetBarbarianPath(gridMatrix, unitCell);
                case UnitType.Poor:
                    return GetPoorPath(gridMatrix, unitCell);
                case UnitType.Shaman:
                    return GetShamanPath(gridMatrix, unitCell);
            }
            return null;
        }
        
        private static List<Vector2Int> GetSwordManPath(Grid<int> gridMatrix, Vector2Int unitCell)
        {
            var neighbours = new List<Vector2Int>();
            CheckStraightOneCell(gridMatrix, unitCell, new[] {0}, ref neighbours);
            return neighbours.Any()? neighbours : null;
        }

        private static List<Vector2Int> GetHorseManPath(Grid<int> gridMatrix, Vector2Int unitCell)
        {
            var neighbours = new List<Vector2Int>();
            CheckDiagonals(gridMatrix, unitCell, new [] {0}, ref neighbours);
            return neighbours.Any()? neighbours : null;
        }

        private static List<Vector2Int> GetAngelPath(Grid<int> gridMatrix, Vector2Int unitCell)
        {
            var neighbours = new List<Vector2Int>();
            var ignoreCells = new int[] {0, 2};
            CheckHorizontal(gridMatrix, unitCell, ignoreCells, ref neighbours);
            CheckVertical(gridMatrix, unitCell, ignoreCells, ref neighbours);
            return neighbours.Any()? neighbours : null;
        }

        private static List<Vector2Int> GetBarbarianPath(Grid<int> gridMatrix, Vector2Int unitCell)
        {
            var neighbours = new List<Vector2Int>();
            var obstaclesToIgnore = new int[] {0, 1};
            CheckVertical(gridMatrix, unitCell, obstaclesToIgnore, ref neighbours);
            CheckHorizontal(gridMatrix, unitCell, obstaclesToIgnore, ref neighbours);
            CheckDiagonals(gridMatrix, unitCell, obstaclesToIgnore, ref neighbours);
            return neighbours.Any()? neighbours : null;
        }

        private static List<Vector2Int> GetPoorPath(Grid<int> gridMatrix, Vector2Int unitCell)
        {
            var neighbours = new List<Vector2Int>();
            CheckVertical(gridMatrix, unitCell, new [] {0}, ref neighbours);
            CheckDiagonalsOneCell(gridMatrix, unitCell, new[] {0}, ref neighbours);
            return neighbours.Any()? neighbours : null;
        }

        private static List<Vector2Int> GetShamanPath(Grid<int> gridMatrix, Vector2Int unitCell)
        {
            var neighbours = new List<Vector2Int>();
            var directions = new Vector2Int[] {
                new  ( -2, -1 ), new ( -2, 1 ),
                new  ( -1, -2 ), new  ( -1, 2 ),
                new  ( 1, -2 ), new  ( 1, 2 ),
                new  ( 2, -1 ), new ( 2, 1 )
            };
            foreach (var direction in directions)
            {
                var neighbourCell = direction + unitCell;
                if(neighbourCell.x >= 0 && neighbourCell.x < gridMatrix.Width 
                                        && neighbourCell.y >= 0 && neighbourCell.y < gridMatrix.Height 
                                        && gridMatrix[neighbourCell] == 0)
                    neighbours.Add(neighbourCell);
            }
            return neighbours.Any()? neighbours : null;
        }

        private static void CheckHorizontal(Grid<int> gridMatrix, Vector2Int unitCell, int[] obstaclesToIgnore, ref List<Vector2Int> neighbours)
        {
            for (int directionCycled = -1; directionCycled <= 1; directionCycled+= 2)
            {
                var startX = unitCell.x + directionCycled;
                var startY = unitCell.y;
                while (startX >= 0 && startX < gridMatrix.Width && obstaclesToIgnore.Contains(gridMatrix[startX,startY]))
                {
                    neighbours.Add(new Vector2Int(startX,startY));
                    startX += directionCycled;
                }
            }
        }

        private static void CheckVertical(Grid<int> gridMatrix, Vector2Int unitCell, int[] obstaclesToIgnore, ref List<Vector2Int> neighbours)
        {
            for (int directionCycled = -1; directionCycled <= 1; directionCycled+= 2)
            {
                var startX = unitCell.x;
                var startY = unitCell.y + directionCycled;
                while (startY >= 0 && startY < gridMatrix.Height && obstaclesToIgnore.Contains(gridMatrix[startX,startY]))
                {
                    neighbours.Add(new Vector2Int(startX,startY));
                    startY += directionCycled;
                }
            }
        }

        private static void CheckDiagonals(Grid<int> gridMatrix, Vector2Int unitCell, int[] obstaclesToIgnore,
            ref List<Vector2Int> neighbours)
        {
            for (int dirrectionX = -1; dirrectionX <= 1; dirrectionX+=2)
            {
                for (int dirrectionY = -1; dirrectionY <= 1; dirrectionY+=2)
                {
                    int startX = unitCell.x + dirrectionX;
                    int startY = unitCell.y + dirrectionY;
                    while (startX >= 0 && startX < gridMatrix.Width && 
                           startY >= 0 && startY < gridMatrix.Height&&
                           obstaclesToIgnore.Contains(gridMatrix[startX,startY]))
                    {
                        neighbours.Add(new Vector2Int(startX,startY));
                        startX += dirrectionX;
                        startY += dirrectionY;
                    }
                }
            }
        }

        private static void CheckStraightOneCell(Grid<int> gridMatrix, Vector2Int unitCell, int[] obstaclesToIgnore,
            ref List<Vector2Int> neighbours)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            for (var i = 0; i < 4; i++)
            {
                int startX = unitCell.x + dx[i];
                int startY = unitCell.y + dy[i];
                if (startX >= 0 && startX < gridMatrix.Width && startY >= 0 && startY < gridMatrix.Height && obstaclesToIgnore.Contains(gridMatrix[startX,startY]))
                {
                    neighbours.Add(new Vector2Int(startX,startY));
                }
            }
        }

        private static void CheckDiagonalsOneCell(Grid<int> gridMatrix, Vector2Int unitCell, int[] obstaclesToIgnore,
            ref List<Vector2Int> neighbours)
        {
            for (int directionX = -1; directionX <= 1; directionX += 2)
            {
                for (int directionY = -1; directionY <= 1; directionY += 2)
                {
                    var startX = unitCell.x + directionX;
                    var startY = unitCell.y + directionY;
                    if (startX >= 0 && startX < gridMatrix.Width && startY >= 0 && startY < gridMatrix.Height &&
                        obstaclesToIgnore.Contains(gridMatrix[startX, startY]))
                    {
                        neighbours.Add(new Vector2Int(startX,startY));
                    }
                }
            }
        }
    }
}