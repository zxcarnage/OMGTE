using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using App.Scripts.Scenes.SceneHeroes.Features.PathFinding.Factory;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public class ServiceUnitNavigator : IServiceUnitNavigator
    {
        public List<Vector2Int> FindPath(UnitType unitType, Vector2Int from, Vector2Int to, Grid<int> gridMatrix)
        { 
            return AStart(unitType, from, to, gridMatrix);
        }

        private List<Vector2Int> AStart(UnitType unitType, Vector2Int fromCell, Vector2Int toCell, Grid<int> gridMatrix)
        {
            var closed = new List<Vector2Int>();
            var open = new List<Vector2Int>{fromCell};
            var unclosedNeighbours = new List<Vector2Int>();
            var from = new Dictionary<Vector2Int, Vector2Int>();
            var gWeight = new Dictionary<Vector2Int, int>();
            var fWeight = new Dictionary<Vector2Int, int>();
            gWeight.Add(fromCell, 0);
            fWeight.Add(fromCell, gWeight[fromCell] + CalculateEuristic(fromCell, toCell));
            while (open.Any())
            {
                var current = MinF(open, fWeight);
                if (current == toCell) 
                    return ReconstructPath(fromCell, toCell, from);
                open.Remove(current);
                closed.Add(current);
                unclosedNeighbours = FindUnclosedNeighbours(unitType, current, gridMatrix, closed);
                foreach (var unclosedNeighbour in unclosedNeighbours)
                {
                    var tempGWeight = gWeight[current] + ManhattanDistance(current, unclosedNeighbour);
                    if (!open.Contains(unclosedNeighbour) || tempGWeight < gWeight[unclosedNeighbour])
                    {
                        from[unclosedNeighbour] = current;
                        gWeight[unclosedNeighbour] = tempGWeight;
                        fWeight[unclosedNeighbour] =
                            gWeight[unclosedNeighbour] + CalculateEuristic(unclosedNeighbour, toCell);
                    }
                    if(!open.Contains(unclosedNeighbour)) open.Add(unclosedNeighbour);
                }
            }
            return null;
        }

        private Vector2Int MinF(List<Vector2Int> list, Dictionary<Vector2Int, int> fWeight)
        {
            var tempElement = list[0];
            foreach (var element in list)
            {
                tempElement = fWeight[element] < fWeight[tempElement] ? element : tempElement;
            }

            return tempElement;
        }

        private int CalculateEuristic(Vector2Int fromCell, Vector2Int toCell)
        {
            return (int) Mathf.Sqrt(Mathf.Pow(toCell.x - fromCell.x, 2) + Mathf.Pow(toCell.y - fromCell.y, 2));
        }

        private List<Vector2Int> FindUnclosedNeighbours(UnitType unitType, Vector2Int unitCell, Grid<int> gridMatrix, List<Vector2Int> closedNeighbours)
        {
            INeighbourPathFactory neighbourPathFactory = new NeighbourPathFactory();
            return neighbourPathFactory
                .ReceiveNeighbours(unitType, unitCell, gridMatrix)
                .Except(closedNeighbours)
                .ToList();
        }

        private int ManhattanDistance(Vector2Int from, Vector2Int to)
        {
            return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
        }
        
        private List<Vector2Int> ReconstructPath(Vector2Int fromCell, Vector2Int toCell, Dictionary<Vector2Int, Vector2Int> from)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int currentCell = toCell;
            
            while (currentCell != fromCell)
            {
                path.Add(currentCell);
                currentCell = from[currentCell];
            }
            path.Reverse();

            return path;
        }
    }
}