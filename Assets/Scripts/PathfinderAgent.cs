using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.Model;
using UnityEngine;
using Zenject;

public class PathfinderAgent : MonoBehaviour
{
    [SerializeField]
    private Vector3 _targetPosition;

    private Cell _targetCell;

    
    private GridController _gridController;

    private List<Cell> _currentPath;


    private Cell WorldToCell(Vector3 posWorld)
    {
        var posX = Mathf.RoundToInt(posWorld.z);
        var posY = Mathf.RoundToInt(posWorld.x);

        return _gridController.Grid[posX, posY];
    }

    [Inject]
    public void Inject(GridController gc)
    {
        _gridController = gc;
    }

    public void CalculatePath()
    {
        var startCell = WorldToCell(transform.position);
        _targetCell = WorldToCell(_targetPosition);

        var result = CalculatePath(startCell, new List<Cell>());

    }

    public List<Cell> CalculatePath(Cell pos, List<Cell> marked)
    {
        marked.Add(pos);

        if (pos == _targetCell)
            return new List<Cell> {pos};
        else
        {
            var adj = GetAdjacentCells(pos, marked);
            if (adj.Count == 0)
                return null;
            foreach (var cell in adj)
            {
                var potPath = CalculatePath(cell, marked);
                if (potPath != null)
                {
                    potPath.Add(pos);
                    return potPath;
                }
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
            try
            {
                var absX = Mathf.RoundToInt(x + pos.Position.x);
                var absY = Mathf.RoundToInt(y + pos.Position.y);

                var candidate = _gridController.Grid[absX, absY];

                if (candidate == null)
                    continue;

                if (!marked.Contains(candidate) 
                        &&  candidate.Type != CellType.Wall)
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

    private void FollowPath()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        CalculatePath();

    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
    }
}
