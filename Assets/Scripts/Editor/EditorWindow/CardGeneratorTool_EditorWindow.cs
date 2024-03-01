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

    GameObject cardToGenerate = null;

    string cardPrefabName = "";
    [MenuItem("Tools/Card Generator")]
    public static void ShowWindow()
    {
        GetWindow<CardGeneratorTool_EditorWindow>("Card Generator").Show();

    }

    private void OnGUI()
    {

        
        GUILayout.BeginVertical();

        cardPrefabName = EditorGUILayout.TextField("Card Prefab File Name", cardPrefabName);
        
        GUILayout.EndVertical();

        bool _createCard = GUILayout.Button("Generate Card");
        if (_createCard)
        {
            CreateFolder("Prefabs", "CardPrefabs");
            Selection.activeGameObject = cardToGenerate;
            UnpackToolEditor.UnpackCompletely();


        }

        cardToGenerate = (GameObject)EditorGUILayout.ObjectField("Card to create", cardToGenerate, typeof(GameObject), true);
        GUIUtility.ExitGUI();
    }


    private void CreateFolder(string _subFolder, string _folderName)
    {
        if (!AssetDatabase.IsValidFolder($"Assets/{_subFolder}"))
        {
            AssetDatabase.CreateFolder($"Assets", $"{_subFolder}");
        }


        if (!AssetDatabase.IsValidFolder($"Assets/{_subFolder}/{_folderName}"))
            AssetDatabase.CreateFolder($"Assets/{_subFolder}", $"{_folderName}");

        string _prefabPath = $"Assets/Prefabs/{_folderName}/{cardPrefabName}.prefab";

        SaveCreatedAssets(_prefabPath);

    }

    private void GeneratePrefabPath(string _folderName)
    {
        string _prefabPath = $"Assets/Prefabs/{_folderName}/{cardPrefabName}.prefab";

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
        if (cardToGenerate != null)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(cardToGenerate) && !PrefabUtility.IsAnyPrefabInstanceRoot(cardToGenerate))
            {
                Debug.Log("Created card");
                GameObject _cardPrefabVariant = PrefabUtility.SaveAsPrefabAsset(cardToGenerate, $"{_prefabPath}");
                PrefabUtility.InstantiatePrefab(_cardPrefabVariant);
                //Selection.activeGameObject = _cardPrefabVariant;
                UnpackToolEditor.UnpackCompletely();
                AssetDatabase.Refresh();
            }
        }
    }

}
