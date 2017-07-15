using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Creates a map based on a csv file. 
/// A row is mapped on the z axis, a column on the x axis
/// </summary>
public class MapGenerator : EditorWindow {

    private List<GameObject> _tiles = new List<GameObject>();
    private int[,] _grid;
    private string _fileName = "test";
    private string _path = "Assets/Resources/test.csv";
    private string _rotationPath = "Assets/Resources/rotation.csv";

    [MenuItem("Window/MapGenerator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapGenerator window = (MapGenerator)GetWindow(typeof(MapGenerator));
        window.Show();
    }

    // Update is called once per frame
    void OnGUI() {
        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i] = (GameObject)EditorGUILayout.ObjectField(i.ToString(),_tiles[i],typeof(GameObject),true);
        }
        if (GUILayout.Button("add prefab"))
        {
            _tiles.Add(null);
        }
        _rotationPath = EditorGUILayout.TextField("Rotation path", _rotationPath);
        _path = EditorGUILayout.TextField("CSV path", _path);
        _fileName = EditorGUILayout.TextField("Prefab Name", _fileName);
        if (GUILayout.Button("Generate")) {

            string csv = System.IO.File.ReadAllText(_path);
            string[] lines = csv.Split("\n"[0]);
            string csvRotation = System.IO.File.ReadAllText(_rotationPath);
            string[] rotationlines = csvRotation.Split("\n"[0]);
            int x = lines.Length;
            int y = lines[0].Split(";"[0]).Length;
            Debug.Log(x);
            Debug.Log(y);

            _grid = new int[x, y];

            GameObject parent = new GameObject("Map");
            for (int i = 0; i<x;i++) {
                string[] line = lines[i].Split(";"[0]);
                string[] rotationLine = rotationlines[i].Split(";"[0]);

                if (line.Length == y) {
                    for (int j = 0; j<y;j++) {
                        int parsedCell = 0;
                        int parsedRotationCell = 0;
                        int.TryParse(rotationLine[j], out parsedRotationCell);
                        if (int.TryParse(line[j], out parsedCell)) {
                            if (parsedRotationCell == -1) {
                                parsedRotationCell = UnityEngine.Random.Range(0, 3) * 90;
                            }
                            
                            _grid[i, j] = parsedCell;
                            if (_grid[i, j] != -1)
                            {
                                GameObject g = _tiles[_grid[i, j]];
                                Instantiate(g, new Vector3(i, 0, j), Quaternion.Euler(0, parsedRotationCell, 0), parent.transform);
                            }
                        }
                    }
                }
            }
            PrefabUtility.CreatePrefab("Assets/Prefabs/"+_fileName+".prefab", parent);
            DestroyImmediate(parent,true);
        }
		
	}
}
