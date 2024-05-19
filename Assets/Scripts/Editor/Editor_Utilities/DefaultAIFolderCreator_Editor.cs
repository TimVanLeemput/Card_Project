using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DefaultAIFolderCreator_Editor
{
    /// <summary>
    /// This method generated 
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
}
