using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreatePrefabWithListModel))]
public class CreatePrefabCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CreatePrefabWithListModel createPrefabWithListModel = (CreatePrefabWithListModel)target;

        if (GUILayout.Button("Create Prefab"))
        {
            createPrefabWithListModel.CreatePrefabInternal();
        }
    }
}
