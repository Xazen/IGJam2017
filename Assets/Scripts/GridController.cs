using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Model;
using UnityEngine;
using Zenject;

public class GridController: IInitializable
{

    public Cell[,] Grid { get; set; }


    [SerializeField] private string _pathToMapFile;

    private readonly Dictionary<string, CellType> _symbolToCellMapping = new Dictionary<string, CellType>
    {
        {"0", CellType.Wall },
        {"1", CellType.Street },
    };

    public void Initialize()
    {
        Grid = ParseMap(_pathToMapFile);
    }



    private Cell[,] ParseMap(string mapFilePath)
    {
        var mapRows = File.ReadAllLines(mapFilePath);
        var mapHeight = mapRows.Length + 1;
        var mapWidth = mapRows[0].Split(';').Length + 1;
        var result = new Cell[mapWidth, mapHeight];

        int x;
        int y = 0;
        foreach (var row in mapRows)
        {
            y++;
            var cellSymbols = row.Split(';');
            x = 0;
            foreach (var cellSymbol in cellSymbols)
            {
                x++;
                CellType celltype;
                if (_symbolToCellMapping.TryGetValue(cellSymbol, out celltype))
                    result[x, y] = new Cell { Position = new Vector2(x, y), Type = celltype };
            }
        }
        return result;
    }
}
