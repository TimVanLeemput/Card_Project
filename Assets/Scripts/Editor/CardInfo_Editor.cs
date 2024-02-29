using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(CardInfo))]

public class CardInfo_Editor : Editor
{
    [SerializeField] public event Action<string> onFolderCreated = null;
    [SerializeField] public event Action<string> onPathCreated = null;
    [SerializeField] GameObject cardToGenerate = null;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bool _prefabFolder = GUILayout.Button("Generate Card");
        if (_prefabFolder)
        { 
            CreateFolder("Prefabs", "CardPrefabs");

        }
        cardToGenerate = (GameObject)EditorGUILayout.ObjectField("Card to create", cardToGenerate, typeof(GameObject),true);

        bool _generateCard = GUILayout.Button("Generate Card");
        if (_generateCard)
        {
            //GenerateCardPrefab();
        }
    }


    private void CreateFolder(string _subFolder, string _folderName)
    {
        if (!AssetDatabase.IsValidFolder($"Assets/{_subFolder}"))
        {
            AssetDatabase.CreateFolder($"Assets", $"{_subFolder}");
        }
           

        if (!AssetDatabase.IsValidFolder($"Assets/{_subFolder}/{_folderName}"))
            AssetDatabase.CreateFolder($"Assets/{_subFolder}", $"{_folderName}");

        onFolderCreated?.Invoke(_folderName);
        onFolderCreated -= GeneratePrefabPath;
        onFolderCreated += GeneratePrefabPath;

    }

    private void GeneratePrefabPath(string _folderName)
    {
        //string _gameObjectPath = $"Assets/GameObjects/{_folderName}/MyGameObject01.gameobject";
        string _prefabPath = $"Assets/Prefabs/{_folderName}/myCard1.prefab";
        
        onPathCreated?.Invoke(_prefabPath);
        onPathCreated += DeleteIfExists;
        onPathCreated += SaveCreatedAssets;
    }

    private void DeleteIfExists(string _prefabPath)
    {
        //AssetDatabase.DeleteAsset(_gameObjectPath);
        //AssetDatabase.DeleteAsset(_prefabPath);
    }

    private void SaveCreatedAssets(string _prefabPath)
    {
        if (cardToGenerate != null)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(cardToGenerate) && !PrefabUtility.IsAnyPrefabInstanceRoot(cardToGenerate))
            {
                Debug.Log("Created card");
                // Create a prefab variant from the GameObject
                //GameObject prefabVariant = PrefabUtility.SaveAsPrefabAssetAndConnect(cardToGenerate, $"Assets/GameObjects/CardGameObjects/new.prefab", InteractionMode.UserAction);
                GameObject prefabVariant = PrefabUtility.SaveAsPrefabAssetAndConnect(cardToGenerate, $"{_prefabPath}", InteractionMode.UserAction);
            }
        }
        //    //// Save the card as an asset.
        //    AssetDatabase.CreateAsset(cardToGenerate, _prefabPath);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
    }

    private void GenerateCard()
    {
        
        //// Create the mesh somehow.
        //Mesh mesh = GetMyMesh();

      
            
        //// Create a transform somehow, using the mesh that was previously saved.
        //Transform trans = GetMyTransform(mesh);

        //// Save the transform's GameObject as a prefab asset.
        //PrefabUtility.CreatePrefab(prefabPath, trans.gameObject);

    }
}
