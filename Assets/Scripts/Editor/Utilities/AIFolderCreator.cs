using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AIFolderCreator_Editor
{
    public static Action<Texture2D,string,string> OnAIMatAndTexturePathsCreated = null; //AI Generated texture, texturePath,matPath
    
    /// <summary>
    /// This method generates and/or checks the default folders to 
    /// place the AI generated mats and textures in.
    /// </summary>
    public static void CreateDefaultAIMatsFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats"))
        {
            AssetDatabase.CreateFolder("Assets/Materials", "AI_Mats");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats/Textures"))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats", "Textures");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Materials/AI_Mats/Materials"))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats", "Materials");
        }
    }
    /// <summary>
    /// This method generates 2 paths : one for the texture and for the material
    /// Could possible be transformed in a method with two out strings : 
    /// void func(Texture2D _tex, out string _texPath, out string _matPath)
    /// </summary>
    /// <param name="_texture"> The AI generated texture</param>
    public static void CreateAIMaterialsFolders(Texture2D _AI_GeneratedTexture)
    {
        CreateDefaultAIMatsFolders(); // Ensures the creation of the default root folders for materials, textures and AI_Mats
        Texture2D _generatedTexture = _AI_GeneratedTexture;
        string _userInputNoSpace = $"{AI_ImageGenerator_EditorWindow.userInputPrompt.Replace(" ", "_")}";  // Replacing spaces with underscores
        _userInputNoSpace = _userInputNoSpace.Replace("\"", "");  // Replacing double quotes with nothing
        _userInputNoSpace = _userInputNoSpace.Replace("\'", ""); // Replacing single quotes with nothing
        string _fullPathTextures = $"Assets/Materials/AI_Mats/Textures/{_userInputNoSpace}_Textures";
        if (!AssetDatabase.IsValidFolder(_fullPathTextures))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats/Textures", $"{_userInputNoSpace}_Textures"); // Doesn't allow numbers in path!
        }

        string _fullPathMaterials = $"Assets/Materials/AI_Mats/Materials/{_userInputNoSpace}_Material";
        if (!AssetDatabase.IsValidFolder(_fullPathMaterials))
        {
            AssetDatabase.CreateFolder("Assets/Materials/AI_Mats/Materials", $"{_userInputNoSpace}_Material"); // Doesn't allow numbers in path!
        }

        string _newTexturePath = $"{_fullPathTextures}/{AI_ImageGenerator_EditorWindow.userInputPrompt}_Downloaded_2D_Texture.asset";

        string _newMatPath = $"{_fullPathMaterials}/{AI_ImageGenerator_EditorWindow.userInputPrompt}_Downloaded_Material.mat";
        _newTexturePath = AssetDatabase.GenerateUniqueAssetPath(_newTexturePath);
        _newMatPath = AssetDatabase.GenerateUniqueAssetPath(_newMatPath);
        OnAIMatAndTexturePathsCreated?.Invoke(_generatedTexture, _newTexturePath, _newMatPath);
    }

}
