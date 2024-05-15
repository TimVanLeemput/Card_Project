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

    private static string FOLDER_PATH = "";
    private static string NEW_ENTRY_TITLE = "All flavor texts for : ";
    /// <summary>
    /// Use this method to create a folder
    /// </summary>
    public static void CreateFolder(string _folderName)
    {
        if (!AssetDatabase.IsValidFolder($"Assets/Resources/AI_Texts/{_folderName}"))
        {
            string _path = AssetDatabase.CreateFolder($"Assets/Resources/AI_Texts", $"{_folderName}");

            SetFolderPath($"{_path}");
        }
        else
        {
            SetFolderPath($"Assets/Resources/AI_Texts/{_folderName}");
            //Debug.Log($"Folder set to FOLDER_PATH ==> {FOLDER_PATH}");
        }
    }

    private static void SetFolderPath(string _folderPath)
    {
        FOLDER_PATH = _folderPath;
    }

    /// <summary>
    /// Use this method to add text to the current text file
    /// </summary>
    public  static void AddToTextFile(string _textToAdd, string _entryTitle = "")
    {
        try
        {
            // Load the text asset
            string _assetPath = $"{FOLDER_PATH}/{_entryTitle}.csv";
            string _contentString = string.Empty;

            if (File.Exists(_assetPath))
            {
                // Read existing content if the file exists
                _contentString = File.ReadAllText(_assetPath);
            }
            else
            {
                // Create a new text file if it doesn't exist
                File.WriteAllText(_assetPath, "");
            }

            // Check if entry title is provided and it doesn't already exist in the content
            if (!string.IsNullOrEmpty(_entryTitle) && !_contentString.Contains($"{NEW_ENTRY_TITLE}{_entryTitle}"))
            {
                _contentString += $"\n{NEW_ENTRY_TITLE}{_entryTitle}\n\n";
            }

            // Append the new text
            _contentString += $"{_textToAdd}\n";

            // Write the updated content back to the asset file
            File.WriteAllText(_assetPath, _contentString);

            // Refresh the asset database to reflect the changes
            AssetDatabase.Refresh();

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
