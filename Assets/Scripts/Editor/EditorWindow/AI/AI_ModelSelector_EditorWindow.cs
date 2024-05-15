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

    public static void SelectChatModelField()
    {
        if (AI_ModelSelector_Editor.allChatModels == null)
        {
            AI_ModelSelector_Editor.SetAllChatModels();
        }
        GUIHelpers_Editor.SpaceV(5);
        GUILayout.BeginVertical();
        List<Model> _allModels = AI_ModelSelector_Editor.allChatModels;

        int _size = _allModels.Count;
        EditorGUI.BeginChangeCheck();
        List<string> _allModelsID = new List<string>();

        foreach (Model _model in _allModels)
        {
            _allModelsID.Add(_model.ModelID);
        }
        if (_allModels.Count > 0)
        {
            currentModelIndex = EditorGUILayout.Popup("Select AI Model",
                currentModelIndex, _allModelsID.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                Model _selectedModel = _allModels[currentModelIndex];
                AI_ModelSelector_Editor.SetChatModel(_selectedModel);
            }
        }

        GUILayout.EndVertical();
        GUIHelpers_Editor.SpaceV(5);
    }
}
