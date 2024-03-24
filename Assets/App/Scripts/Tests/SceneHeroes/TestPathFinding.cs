using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Serializable;
using App.Scripts.Scenes.SceneHeroes.Features.PathFinding;
using App.Scripts.Scenes.SceneHeroes.Features.PathFinding.Factory;
using NUnit.Framework;
using UnityEngine;

public class TestPathFinding
{
    private const string PathTest = "Assets/App/Scripts/Tests/SceneHeroes/TestCases/{0}.json";
    private const string PathFactoryTest = "Assets/App/Scripts/Tests/SceneHeroes/TestCases/FactoryTest/{0}.json";

    [Test]
    [TestCase("test_field_path(0)")]
    [TestCase("test_field_path(1)")]
    [TestCase("test_field_path(2)")]
    [TestCase("test_field_path(3)")]
    [TestCase("test_field_path(4)")]
    [TestCase("test_field_path(5)")]
    [TestCase("test_field_path(6)")]
    [TestCase("test_field_path(7)")]
    [TestCase("test_field_path(8)")]
    [TestCase("test_field_path(9)")]
    public void TestPathFindingSimplePasses(string testData)
    {
        var serviceUnitNavigator = new ServiceUnitNavigator();
    
        var testCaseText = File.ReadAllText(string.Format(PathTest, testData));

        var testCase = JsonUtility.FromJson<LevelInfoTarget>(testCaseText);
        
        var grid = new Grid<int>(testCase.gridSize.ToVector2Int());

        foreach (var obstacle in testCase.Obstacles)
        {
            grid[obstacle.Place.ToVector2Int()] = obstacle.ObstacleType;
        }
    
        var path = serviceUnitNavigator.FindPath(testCase.UnitType, testCase.PlaceUnit.ToVector2Int(), 
            testCase.target.ToVector2Int(), grid);

        if (testCase.targetStepCount < 0 && path is null)
        {
            return;   
        }

        Assert.AreEqual(testCase.targetStepCount, path.Count,"step count invalid");
    }

    [Test]
    [TestCase("factory_test_0")]
    [TestCase("factory_test_1")]
    [TestCase("factory_test_2")]
    [TestCase("factory_test_3")]
    [TestCase("factory_test_4")]
    [TestCase("factory_test_5")]
    public void TestNeighbourFactory(string testData)
    {
        INeighbourPathFactory neighbourPathFactory = new NeighbourPathFactory();
        List<Vector2Int> targetNeighbours = new List<Vector2Int>();

        var testCaseText = File.ReadAllText(string.Format(PathFactoryTest, testData));

        var testCase = JsonUtility.FromJson<FactoryTestInfo>(testCaseText);
        
        var grid = new Grid<int>(testCase.gridSize.ToVector2Int());

        var unitType = testCase.UnitType;
        
        foreach (var obstacle in testCase.Obstacles)
        {
            grid[obstacle.Place.ToVector2Int()] = obstacle.ObstacleType;
        }

        foreach (var targetNeighbour in testCase.TargetNeighbours)
        {
            targetNeighbours.Add(targetNeighbour.ToVector2Int());
        }

        var unitCell = testCase.PlaceUnit.ToVector2Int();

        var neighbours = neighbourPathFactory.ReceiveNeighbours(unitType, unitCell, grid);
        
        Assert.That(neighbours , Is.EquivalentTo(targetNeighbours),"Neighbours are invalid!");
    }

}
