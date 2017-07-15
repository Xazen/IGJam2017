using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.Model;
using UnityEngine;
using Zenject;

public class GridController: IInitializable
{
    public Cell[,] Grid { get; set; }

    private const string _pathToMapFile = "Assets/Resources/test.csv";

    private readonly Dictionary<string, CellType> _symbolToCellMapping = new Dictionary<string, CellType>
    {
        {"0", CellType.Wall },
        {"1", CellType.Street },
    };

    public void Initialize()
    {
        Grid = ParseMap(_pathToMapFile);
    }

    public CellType GetCellType(int x, int y)
    {
        return Grid[x, y].Type;
    }

    private Cell[,] ParseMap(string mapFilePath)
    {
        var mapRows = File.ReadAllLines(mapFilePath);
        var mapHeight = mapRows.Length;
        var mapWidth = mapRows[0].Split(';').Length;
        var result = new Cell[mapWidth, mapHeight];

        int x;
        int y = 0;
        foreach (var row in mapRows)
        {
            var cellSymbols = row.Split(';');
            x = 0;
            foreach (var cellSymbol in cellSymbols)
            {
                CellType celltype;
                if (_symbolToCellMapping.TryGetValue(cellSymbol, out celltype))
                    result[x, y] = new Cell { Position = new Vector2(x, y), Type = celltype };
                x++;
            }
            y++;
        }
        return result;
    }
}
