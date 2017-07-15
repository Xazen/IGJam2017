using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : EditorWindow {

    private List<GameObject> tiles = new List<GameObject>();
    private int[,] grid;
    private string fileName = "test";
    private string path;

    [MenuItem("Window/MapGenerator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapGenerator window = (MapGenerator)GetWindow(typeof(MapGenerator));
        window.Show();
    }

    void Update() {

    }

    // Update is called once per frame
    void OnGUI() {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i] = (GameObject)EditorGUILayout.ObjectField(i.ToString(),tiles[i],typeof(GameObject),true);
        }
        if (GUILayout.Button("add prefab"))
        {
            tiles.Add(null);
        }
        path = EditorGUILayout.TextField("CSV path", path);
       
        fileName = EditorGUILayout.TextField("Prefab Name", fileName);
        if (GUILayout.Button("Generate")) {

            string csv = System.IO.File.ReadAllText(path);
            string[] lines = csv.Split("\n"[0]);
            int x = lines.Length;
            int y = lines[0].Split(";"[0]).Length;
            Debug.Log(x);
            Debug.Log(y);

            grid = new int[x, y];

            GameObject parent = Instantiate(new GameObject("Map"));
            for (int i = 0; i<x;i++) {
                string[] line = lines[i].Split(";"[0]);
                if (line.Length == y) {
                    for (int j = 0; j<y;j++) {
                            int a = 0;
                            if (int.TryParse(line[j], out a)) {
                                grid[i, j] = a;
                                if (grid[i, j] != -1) {
                                    Instantiate(tiles[grid[i, j]],new Vector3(i,0,j),Quaternion.identity,parent.transform);
                            }
                        }
                    }
                }
            }
            PrefabUtility.CreatePrefab("Assets/Prefabs/"+fileName+".prefab", parent);
            DestroyImmediate(parent);
        }
		
	}
}
