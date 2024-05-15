using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public static class AI_ModelSelector
{

    public static List<Model> allChatModels = null;
    public static AI_Card_Tool_EditorWindow tool = null;

    
    public static void SetTool(AI_Card_Tool_EditorWindow _tool)
    {
        tool = _tool;
    }

    public static void SetAllChatModels()
    {
        allChatModels = new List<Model>();
    }

    /// <summary>
    /// Call this method to define the Chat Model of the type Conversation
    /// </summary>
    /// <param name="_conversation"></param>
    /// <param name="_selectedModel"></param>
    public static void SetChatModel(Model _model)
    {
        if (tool == null || _model == null)
        {
            Debug.Log($"FAILED TO FIND TOOL OR MODEL in {typeof(AI_ModelSelector)}");
            return;
        }
        tool.Conversation.Model = _model;

        Debug.Log($"{_model.ModelID} is now the selected AI MODEL in SetChatModel ({typeof(AI_ModelSelector)})");

    }
    public static void ChatModelSelection()
    {
        if (tool == null) return;
        List<Model> _allModels = Model.PopulateModels();

        allChatModels = _allModels;
    }
}
