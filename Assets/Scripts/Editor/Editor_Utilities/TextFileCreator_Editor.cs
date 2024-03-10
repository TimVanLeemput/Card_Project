using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TextFileCreator_Editor
{

    private static string folderPath = "";
    /// <summary>
    /// Use this method to create a folder
    /// </summary>
    //public static void CreateFolder(string _folderName)
    //{
    //    if (!AssetDatabase.IsValidFolder($"Assets/{_folderName}"))
    //    {
    //        AssetDatabase.CreateFolder($"Assets", $"{_folderName}");

    //    }
    //    SetFolderPath($"Assets/{_folderName}");
    //}

    //private static void SetFolderPath(string _folderPath)
    //{
    //    folderPath = _folderPath;
    //}

    /// <summary>
    /// Use this method to add text to the current text file
    /// </summary>
    public static void AddToTextFile(string _textToAdd)
    {
        try
        {
            TextAsset _asset = Resources.Load<TextAsset>("AI_Texts");
            if (_asset == null)
            {
                Debug.LogError("Failed to load text asset");
                return;
            }

            string _contentString = _asset.text;
            _contentString += $"{_textToAdd}" + "\n";

            // Write the updated content back to the asset file
            string assetPath = AssetDatabase.GetAssetPath(_asset);
            File.WriteAllText(assetPath, _contentString);

            // Refresh the asset database to reflect the changes
            AssetDatabase.Refresh();

            Debug.Log("Text file updated successfully");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating text file: " + ex.Message);
        }
    }


    /// <summary>
    /// Use this method to add a text to an already existing text file
    /// </summary>
    /// <param name="_textToAddToFile"></param>
    public static void AddTextToFile(string _textToAddToFile)
    {

    }

    private void CreateAndSaveTextAsset()
    {
        //// Create a new TextAsset
        //TextAsset newAsset = new TextAsset();

        //// Set the content of the TextAsset
        //newAsset.text = "Your default content here";

        //// Get the path where to save the asset
        //string assetPath = "Assets/Resources/" + TEMPLATE_CLASS_FILENAME + ".txt";

        //// Write the TextAsset to a file
        //File.WriteAllText(assetPath, newAsset.text);

        //// Refresh the asset database to ensure it's visible in the editor
        //UnityEditor.AssetDatabase.Refresh();
    }
}
