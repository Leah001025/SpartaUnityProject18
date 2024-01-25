using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(MapGeneratorBase), true)]
public class RandomDungeonGenerateEditor : Editor
{ 
    MapGeneratorBase generator;

    private void Awake()
    {
        generator = (MapGeneratorBase)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
