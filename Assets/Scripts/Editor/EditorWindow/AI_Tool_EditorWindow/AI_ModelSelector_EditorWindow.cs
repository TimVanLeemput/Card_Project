using NUnit.Framework.Constraints;
using OpenAI_API.Models;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// This class functions in conjunction with the AI_ModelSelector class
/// Creates a field to select the AI Chat Model
/// </summary>
public class AI_ModelSelector_EditorWindow : EditorWindow
{
    static int currentModelIndex = 0;
    static List<string> allModelsID = null;

    public static void Init()
    {
        //if (allModelsID.Count > 0) return;
        allModelsID = AI_ModelSelector.GetAllChatModelsIDs();
    }

    public static void SelectChatModelField()
    {
        GUIHelpers.SpaceV(5);
        GUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();

        if (AI_ModelSelector.allChatModels.Count > 0)
        {
          
            currentModelIndex = EditorGUILayout.Popup("Select AI Model",
                currentModelIndex, allModelsID.ToArray(),GUILayout.Width(280));

            if (EditorGUI.EndChangeCheck())
            {
                Model _selectedModel = AI_ModelSelector.allChatModels[currentModelIndex];
                AI_ModelSelector.SetChatModel(_selectedModel);
            }
        }

        GUILayout.EndVertical();
        GUIHelpers.SpaceV(5);
    }
}

