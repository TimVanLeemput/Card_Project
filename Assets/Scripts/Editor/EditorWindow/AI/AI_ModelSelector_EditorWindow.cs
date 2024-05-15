using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class AI_ModelSelector_EditorWindow : EditorWindow
{
    //public static event Action<Model> OnModelSelected = null;
    static int currentModelIndex = 0;

    private void OnEnable()
    {
        //OnModelSelected += SetChatModel;
    }

    private void SetChatModel(Model _model)
    {

    }

    public static void SelectChatModelField()
    {
        if (AI_ModelSelector.allChatModels == null)
        {
            AI_ModelSelector.SetAllChatModels();
        }
        GUIHelpers_Editor.SpaceV(5);
        GUILayout.BeginVertical(); // Begin a vertical layout group
        List<Model> _allModels = AI_ModelSelector.allChatModels;

        int _size = _allModels.Count;
        EditorGUI.BeginChangeCheck();
        List<string> _allModelsID = new List<string>();

        foreach (Model _model in _allModels)
        {
            _allModelsID.Add(_model.ModelID); // Corrected from _allModels.Add(_model.ModelID);

        }
        //_allModelsID.Insert(0, "Select a model");
        if (_allModels.Count > 0)
        {
            // Initialize _currentModelIndex with the index of the first model
            //int _selectedModelIndex = _allModels


            currentModelIndex = EditorGUILayout.Popup("Select AI Model",
                currentModelIndex, _allModelsID.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                Model _selectedModel = _allModels[currentModelIndex];
                AI_ModelSelector.SetChatModel(_selectedModel);
            }
        }

        GUILayout.EndVertical(); // End the vertical layout group
        GUIHelpers_Editor.SpaceV(5);
    }

}
