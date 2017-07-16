using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using UnityEngine;
using Zenject;

public class PathfinderAgent : MonoBehaviour
{
    [SerializeField]
    private Vector3 _targetPosition;

    [SerializeField, Range(0,5)]
    private float _movementSpeed; 

    private Cell _targetCell;

    
    private GridController _gridController;

    private List<Cell> _currentPath;

    private bool _stopMovement;

    private Cell _nextCell;

    private bool IsOnStreet(Vector3 world)
    {
        try
        {
            var posX = Mathf.RoundToInt(world.z);
            var posY = Mathf.RoundToInt(world.x);
            return _gridController.Grid[posX, posY].Type != CellType.Wall;
        }
        catch
        {
            return false;
        }
    }

    private Cell WorldToCell(Vector3 posWorld)
    {
        var posX = Mathf.RoundToInt(posWorld.z);
        var posY = Mathf.RoundToInt(posWorld.x);

        return _gridController.Grid[posX, posY];
    }

    private Vector3 GridToWorldPos(Vector2 grid)
    {
        return new Vector3(grid.y, 0, grid.x);
    }

    private Vector2 WorldToGridPos(Vector3 world)
    {
        return new Vector2(world.z, world.x);
    }

    [Inject]
    public void Inject(GridController gc)
    {
        _gridController = gc;
    }

    public void CalculatePath(Vector3 targetPosition)
    {
        Debug.Assert(IsOnStreet(targetPosition), "target must be on street");
        Debug.Assert(IsOnStreet(transform.position), "agent must be on street");

        _stopMovement = false;
        _targetPosition = targetPosition;

        var startCell = WorldToCell(transform.position);
        _targetCell = WorldToCell(_targetPosition);

        _currentPath = CalculatePath(startCell, new List<Cell>());
        if(_currentPath == null)
            Debug.Log("No path found");
    }


    public List<Cell> CalculatePath(Cell pos, List<Cell> marked)
    {
        marked.Add(pos);

        if (pos == _targetCell)
            return new List<Cell> {pos};

        var adj = GetAdjacentCells(pos, marked);
        if (adj.Count == 0)
            return null;
        adj.Shuffle();
        foreach (var cell in adj)
        {
            var potPath = CalculatePath(cell, marked);
            if (potPath != null)
            {
                potPath.Add(pos);
                return potPath;
            }
        }
        return null;
    }

    private List<Cell> GetAdjacentCells(Cell pos, List<Cell> marked)
    {
        var result = new List<Cell>();
        for(var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        {
                //skip the corners
            if (Math.Abs(x) == Math.Abs(y))
                continue;
                
            try
            {
                var absX = Mathf.RoundToInt(x + pos.Position.x);
                var absY = Mathf.RoundToInt(y + pos.Position.y);

                var candidate = _gridController.Grid[absX, absY];

                if (candidate == null)
                    continue;

                if (!marked.Contains(candidate) 
                        && candidate.Type != CellType.Wall)
                {
                    result.Add(candidate);
                }
            }
            catch (IndexOutOfRangeException)
            {
                    //mute exception
            }
        }
        return result;
    }

    public void Stop()
    {
        _stopMovement = true;
    }

    public void Continue()
    {
        _stopMovement = false;
    }

    private void FollowPath()
    {
        if (_currentPath == null)
            return;

        if (_currentPath.Count == 0)
        {
            Stop();
            return;
        }
        _nextCell = _currentPath[_currentPath.Count - 1];
        if (Vector2.Distance(WorldToGridPos(transform.position), _nextCell.Position) < 0.4)
        {
            _currentPath.Remove(_currentPath.Last());
        }

        var direction = _nextCell.Position - WorldToGridPos(transform.position);
        
        transform.position += GridToWorldPos(direction.normalized * _movementSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_stopMovement)
            FollowPath();
    }
}
