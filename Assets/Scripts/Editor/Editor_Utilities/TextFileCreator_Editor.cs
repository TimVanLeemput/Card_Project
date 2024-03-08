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
    public static void CreateFolder(string _folderName)
    {
        if (!AssetDatabase.IsValidFolder($"Assets/{_folderName}"))
        {
            AssetDatabase.CreateFolder($"Assets", $"{_folderName}");

        }
            SetFolderPath($"Assets/{_folderName}");
    }

    private static void SetFolderPath(string _folderPath)
    {
        folderPath = _folderPath;
    }

    /// <summary>
    /// Use this method to create a .txt file
    /// </summary>
    public static void CreateTextFile(string _textFileName)
    {
        TextAsset _textAsset = Resources.Load<TextAsset>("AI_Texts/AI_Texts");
        if (_textAsset == null)
        {
            Debug.LogError("Failed to load text asset");
            return;
        }

        string _contentString = "Content"; 
        _contentString = _textAsset.text;
        string _newPath = Path.Combine(Application.dataPath, "Resources", "AI_Texts/" + _textFileName + ".txt");

        try
        {
            //StringBuilder _contentBuilder = new StringBuilder(_contentString);
            //_contentBuilder.Append("yes");
            //_contentString = _contentBuilder.ToString();
            File.WriteAllText(_newPath, $"{_contentString}");
            AssetDatabase.Refresh();
            //Debug.Log("Text file created successfully at: " + _newPath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error creating text file: " + ex.Message);
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
