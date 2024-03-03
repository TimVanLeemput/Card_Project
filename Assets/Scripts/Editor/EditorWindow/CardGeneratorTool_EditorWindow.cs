using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;


public class CardGeneratorTool_EditorWindow : EditorWindow
{
    public event Action<string> onFolderCreated = null;
    public event Action<string> onPathCreated = null;
    public string folder = "";
    public string subFolder = "";
    GameObject gameObjectToGenerate = null;

    string prefabName = "";
    [MenuItem("Tools/Prefab Generator")]
    public static void ShowWindow()
    {
        GetWindow<CardGeneratorTool_EditorWindow>("Prefab Generator").Show();

    }

    private void OnGUI()
    {
 
        
       
        GUILayout.FlexibleSpace();
        folder = EditorGUILayout.TextField("Folder Name", folder);
     

        subFolder = EditorGUILayout.TextField("SubFolder Name", subFolder);

        prefabName = EditorGUILayout.TextField("Prefab File Name", prefabName);

        GUILayout.FlexibleSpace();
        bool _createCard = GUILayout.Button("Generate Prefab");
        if (_createCard)
        {
            CreateFolder(folder, subFolder);
            Selection.activeGameObject = gameObjectToGenerate;
            UnpackToolEditor.UnpackCompletely();


        }

        gameObjectToGenerate = (GameObject)EditorGUILayout.ObjectField("Prefab to create", gameObjectToGenerate, typeof(GameObject), true);
        GUIUtility.ExitGUI();
    }


    public void CreateFolder(string _subFolder, string _folderName)
    {
        if (!AssetDatabase.IsValidFolder($"Assets/{_subFolder}"))
        {
            AssetDatabase.CreateFolder($"Assets", $"{_subFolder}");
        }


        if (!AssetDatabase.IsValidFolder($"Assets/{_subFolder}/{_folderName}"))
            AssetDatabase.CreateFolder($"Assets/{_subFolder}", $"{_folderName}");

        string _prefabPath = $"Assets/Prefabs/{_folderName}/{prefabName}.prefab";

        SaveCreatedAssets(_prefabPath);

    }

    private void GeneratePrefabPath(string _folderName)
    {
        string _prefabPath = $"Assets/Prefabs/{_folderName}/{prefabName}.prefab";

        onPathCreated?.Invoke(_prefabPath);
        onPathCreated -= DeleteIfExists;
        onPathCreated += DeleteIfExists;
        onPathCreated -= SaveCreatedAssets;
        onPathCreated += SaveCreatedAssets;
    }

    private void DeleteIfExists(string _prefabPath)
    {
        AssetDatabase.DeleteAsset(_prefabPath);
    }

    private void SaveCreatedAssets(string _prefabPath)
    {
        if (gameObjectToGenerate != null)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(gameObjectToGenerate) && !PrefabUtility.IsAnyPrefabInstanceRoot(gameObjectToGenerate))
            {
                Debug.Log("Created prefab");
                GameObject _cardPrefabVariant = PrefabUtility.SaveAsPrefabAsset(gameObjectToGenerate, $"{_prefabPath}");
                PrefabUtility.InstantiatePrefab(_cardPrefabVariant);
                //Selection.activeGameObject = _cardPrefabVariant;
                UnpackToolEditor.UnpackCompletely();
                AssetDatabase.Refresh();
            }
        }
    }

}
