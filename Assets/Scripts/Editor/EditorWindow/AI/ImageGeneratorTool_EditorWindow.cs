using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImageGeneratorTool_EditorWindow : EditorWindow
{
    [MenuItem("Tools/AI Image Generator")]
    public static void ShowWindow()
    {
        GetWindow<CardGeneratorTool_EditorWindow>("AI Image Generator").Show();


    }
    private void OnGUI()
    {

    }
}
