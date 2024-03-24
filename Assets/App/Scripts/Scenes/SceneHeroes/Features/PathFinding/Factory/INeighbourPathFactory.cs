using System.Collections.Generic;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding.Factory
{
    public interface INeighbourPathFactory
    {
        public List<Vector2Int> ReceiveNeighbours(UnitType unitType, Vector2Int unitCell, Grid<int> gridMatrix);
    }
}