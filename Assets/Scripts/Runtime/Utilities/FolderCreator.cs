using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public  class FolderCreator 
{
    private void StringCompare()
    { 
        string _myString = string.Empty;
        string _myString2 = string.Empty;
        _myString.CompareTo(_myString2);

        string.CompareOrdinal(_myString, _myString2);
        string.Compare(_myString, _myString2, System.StringComparison.OrdinalIgnoreCase);
    }
#if UNITY_EDITOR

    public static string CreateFolder(params string[] _folderNames)
    {
        string _folderPath = "Assets";

        foreach (string _folderName in _folderNames)
        {
            // Append the current folder name to the folder path
            _folderPath += $"/{_folderName}";

            // Check if the folder already exists
            if (!AssetDatabase.IsValidFolder(_folderPath))
            {
                // If not, create the folder
                AssetDatabase.CreateFolder(_folderPath.Substring(7), _folderName); // 7 is to remove "Assets/" from the beginning
            }
        }

        // Return the final folder path
        return _folderPath;
    }
#endif


}
