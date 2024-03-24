using System;
using System.Collections.Generic;
using System.Numerics;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Serializable
{
    [Serializable]
    public class FactoryTestInfo
    {
        public Place gridSize;

        public List<ObstacleSerializable> Obstacles = new ();
        public List<Place> TargetNeighbours = new();

        public Place PlaceUnit;
        public UnitType UnitType;
        
    }
    
    [Serializable]
    public class CellSerializable
    {
        public Place Place;
        
        public Vector2Int CellPosition => Place.ToVector2Int();
    }
}